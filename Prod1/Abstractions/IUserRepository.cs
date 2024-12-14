using btlz.Contracts;
using btlz.Models;

namespace btlz.Abstractions;

public interface IUserRepository 
{
    UsersVm GetUsers();
    UsersVm? GetUserById(int id);
    User AddUser(CreateUserDto dto);
    int UpdateUser(int userId, UpdateUserDto dto);
    void DeleteUser(int id);
    User LoginUser(string login, string password);
    User TryGetUserByIdAndThrowIfNotFound(int id);
}