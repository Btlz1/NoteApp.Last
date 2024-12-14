using AutoMapper;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Database;
using btlz.Exceptions;
using btlz.Models;
using btlz.Settings;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace btlz.Services;

public class UserRepository : IUserRepository 
{
    private readonly btlzDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IJwtTokensRepository _jwtTokensRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    public UserRepository(btlzDbContext dbContext, IMapper mapper, 
        IJwtTokensRepository jwtTokensRepository, IJwtTokenGenerator tokenGenerator) 
        => (_dbContext, _mapper, _jwtTokensRepository, _tokenGenerator)
            = (dbContext, mapper, jwtTokensRepository, tokenGenerator);
    
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

    

    public User AddUser(CreateUserDto dto)
    {
        if (_dbContext.Users.Any(user => user.Login == dto.Login))
        {
            throw new ArgumentException(nameof(dto.Login));
        }
        var user = _mapper.Map<User>(dto);
        _dbContext.Users.Add(user); 
        _dbContext.SaveChanges();
        return user;
    }

    public int UpdateUser(int userId, UpdateUserDto dto) 
    {
        var user = TryGetUserByIdAndThrowIfNotFound(userId);
        
        var updatedUser = _mapper.Map<(int, UpdateUserDto), User>((userId, dto));
        user.Login = updatedUser.Login;
        user.Password = updatedUser.Password;
        _dbContext.SaveChanges();
        return user.Id;
    }
    
    public void DeleteUser(int id)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);
        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
    }
    public User LoginUser(string login,[FromBody] string password)
    {
        var user = _dbContext.Users.FirstOrDefault(user => (user.Login == login));
        if (user is null)
        {
            throw new ArgumentException(nameof(login));
        }
        if (user.Password != password)
        {
            throw new ArgumentException(nameof(password));
        }
        return user;
    }

    public string Generate(User user)
    {
        var token = _tokenGenerator.GenerateToken(user);
        _jwtTokensRepository.Update(user.Id, token);
        return token;
    }
    public User TryGetUserByIdAndThrowIfNotFound(int id)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new UserNotFoundException(id);
        }
        return user;
    }
}