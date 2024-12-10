using AutoMapper;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Database;
using btlz.Exceptions;
using btlz.Models;

namespace btlz.Services;

public class UserRepository : IUserRepository 
{
    private readonly btlzDbContext _dbContext;
    private readonly IMapper _mapper;
    public UserRepository(btlzDbContext dbContext, IMapper mapper) 
        => (_dbContext, _mapper) = (dbContext, mapper);
    
    public UsersVm GetUsers() 
    {
        var listOfUsers = (
            from user in _dbContext.Users
            select new UserVm(user.Id, user.Login, user.Password)
        ).ToList();
        var users = new UsersVm(listOfUsers);
        return users;
    }

    public UsersVm? GetUserById(int id)
    {
    var listOfUsers = (
        from user in _dbContext.Users
        where user.Id == id
        join userId in _dbContext.Users on user.Id equals userId.Id
        select new UserVm(user.Id, user.Login, user.Password)
    ).ToList();
    var notes = new UsersVm(listOfUsers);
        return notes;
    }
    
    public int AddUser(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return user.Id;
    }

    public UserVm UpdateUser(int userId, UpdateUserDto dto) 
    {
        var oldUser = TryGetUserByIdAndThrowIfNotFound(userId);
        oldUser.Login = dto.Login;
        oldUser.Password = dto.Password;
        _dbContext.SaveChanges();
        return new UserVm(oldUser.Id, oldUser.Login, oldUser.Password);
    }
    
    public void DeleteUser(int id)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);
        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
    }
    private User TryGetUserByIdAndThrowIfNotFound(int id)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new UserNotFoundException(id);
        }
        return user;
    }
    
    
}