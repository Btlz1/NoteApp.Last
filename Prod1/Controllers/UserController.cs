using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Models;
using btlz.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop.Infrastructure;

namespace btlz.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : BaseController
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokensRepository _jwtTokensRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    
    public UserController(IUserRepository userRepository, IJwtTokensRepository jwtTokensRepository, IJwtTokenGenerator jwtTokenGenerator)
        => (_userRepository, _jwtTokensRepository, _jwtTokenGenerator) = (userRepository, jwtTokensRepository, jwtTokenGenerator);
    
    [HttpGet]
    public ActionResult<UsersVm> GetUsers()
        => Ok(_userRepository.GetUsers());
    
    [HttpGet("{id}")]
    public ActionResult<UsersVm> GetUserById(int id)
        => Ok(_userRepository.GetUserById(id));

    [AllowAnonymous]
    [HttpPost("register")]
    public ActionResult<string> AddUser(CreateUserDto dto)
    {
        var user = _userRepository.AddUser(dto);
        var token = Generate(user);
        return Ok(token);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateUser(int id, UpdateUserDto dto)
        => Ok(_userRepository.UpdateUser(id, dto));
    
    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        _userRepository.DeleteUser(id);
        return NoContent();
    }
    private string Generate(User user)
    {
        var token = _jwtTokenGenerator.GenerateToken(user);
        _jwtTokensRepository.Update(user.Id, token);
        return token;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<string> Login(string login, string password)
    {
        var user = _userRepository.LoginUser(login, password);
        var token = Generate(user);
        return token;
    }
}