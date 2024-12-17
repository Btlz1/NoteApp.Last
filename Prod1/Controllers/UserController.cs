using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Models;
using btlz.Settings;
using Microsoft.AspNetCore.Authorization;

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
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>>Login(string login,[FromBody] string password)
    {
        var user = await _userRepository.LoginUser(login, password);
        var token = Generate(user);
        return token;
    }
    
    [HttpGet]
    public async Task<ActionResult<UsersVm>> GetUsers()
        => Ok(await _userRepository.GetUsers());
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UsersVm>> GetUserById(int id)
        => Ok(await _userRepository.GetUserById(id));

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<string>> AddUser(CreateUserDto dto)
    {
        var user = await _userRepository.AddUser(dto);
        var token = Generate(user);
        return Ok(token);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, UpdateUserDto dto)
        => Ok(await _userRepository.UpdateUser(id, dto));
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await _userRepository.DeleteUser(id);
        return NoContent();
    }
    
    private string Generate(User user)
    {
        var token = _jwtTokenGenerator.GenerateToken(user);
        _jwtTokensRepository.Update(user.Id, token);
        return token;
    }
}
