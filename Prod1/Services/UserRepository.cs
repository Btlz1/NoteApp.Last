using btlz.Abstractions;
using btlz.Exceptions;
using btlz.Models;


namespace btlz.Services;

public class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new()
    {
        new()
        {
            Id = 0,
            Login = "Beatles1",
            Password = "Fedor"
        },
        new()
        {
            Id = 1,
            Login = "FatherXCtulhu",
            Password = "Leha-God"
        }
    };
    
    public IEnumerable<User> GetUsers() => _users;
    
    public User? GetUserBy(Predicate<User> predicate)
        => _users.FirstOrDefault(user =>  predicate(user));

    public int AddUser(User user)
    {
        var userId = _users.Count;
        _users.Add(new User
        {
            Id = userId,
            Login = user.Login,
            Password = user.Password
        });

        return userId;
    }

    public void UpdateUser(User user)
    {
        var oldUser = TryGetUserByIdAndThrowIfNotFound(user.Id);
        oldUser.Login = user.Login;
    }
    
    public void DeleteUser(int id)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);
        _users.Remove(user);
    }

    private User TryGetUserByIdAndThrowIfNotFound(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new UserNotFoundException(id);
        }
        return user;
    }
}