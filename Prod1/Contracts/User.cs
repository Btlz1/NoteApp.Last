namespace btlz.Contracts;

public record UserVm(int Id, string Login);
public record UserListVm(int Id, string Login);
public record ListOfUsers(List<UserListVm> Users);
public record CreateUserDto(string Login, string Password);
public record UpdateUserDto(string Login);