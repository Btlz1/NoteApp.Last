using btlz.Abstractions;
using btlz.Database;
using btlz.Exceptions;
using btlz.Models;
using Microsoft.AspNetCore.Http.HttpResults;


namespace btlz.Services;

public class UserRepository : IUserRepository
{
    private readonly btlzDbContext _dbContext;
    public UserRepository(btlzDbContext dbContext) 
        => _dbContext = dbContext;
    
    public IEnumerable<User> GetUsers() => _dbContext.Users;

    public User? GetUserById(int id) 
        => _dbContext.Users.FirstOrDefault(user => user.Id == id);
    
    public int AddUser(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return user.Id;
    }

    public void UpdateUser(User user)
    {
        var oldUser = TryGetUserByIdAndThrowIfNotFound(user.Id);
        oldUser.Login = user.Login;
        _dbContext.SaveChanges();
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