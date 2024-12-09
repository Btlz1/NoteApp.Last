namespace btlz.Contracts;

public record UserVm(int Id, string Login);
public record UsersVm(List<UserVm> Users);
public record CreateUserDto(string Login, string Password);
public record UpdateUserDto(string Login, string Password);