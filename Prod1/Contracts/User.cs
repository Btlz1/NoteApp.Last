namespace btlz.Contracts;

public record UserVm(int Id, string Login, string Password);
public record UsersVm(List<UserVm> Users);
public record CreateUserDto(string Login, string Password);
public record UpdateUserDto(string Login, string Password);