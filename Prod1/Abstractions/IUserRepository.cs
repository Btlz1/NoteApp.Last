using btlz.Contracts;
using btlz.Models;

namespace btlz.Abstractions;

public interface IUserRepository
{
    UsersVm GetUsers();
    UsersVm? GetUserById(int id);
    int AddUser(CreateUserDto dto);
    UserVm UpdateUser(int userId, UpdateUserDto dto);
    void DeleteUser(int id);
}