using btlz.Models;

namespace btlz.Abstractions;

public interface IUserRepository
{
    IEnumerable<User> GetUsers();
    User? GetUserBy(Predicate<User> predicate);
    int AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
}