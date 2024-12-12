using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Models;
using btlz.Settings;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace btlz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(
	IJwtTokenGenerator jwtTokenGenerator,
	IJwtTokensRepository jwtTokensRepository) 
    : BaseController
{
    private static readonly ConcurrentBag<User> Users = new();
    
    [AllowAnonymous]
    [HttpPost("register")]
    public string Register(string login, string password)
    {
        if (Users.Any(user => user.Login == login))
        {
            throw new ArgumentException(nameof(login));
        }
        User user = new() { Id = Users.Count, Login = login, Password = password };
        Users.Add(user);
        var token = GenerateAndStoreToken(user);
        
        return token;
    }

    [AllowAnonymous] // Указывает, что этот метод доступен без авторизации
    [HttpPost("login")]
    public string Login(string name,[FromBody] string password)
    {
        var user = Users.FirstOrDefault(user => user.Login == name);
        if (user is null)
        {
            throw new ArgumentException(nameof(name));
        }
        if (user.Password != password)
        {
            throw new ArgumentException(nameof(password));
        }
        var token = GenerateAndStoreToken(user);
        return token;
    }

    [HttpDelete("logout")]
    public IActionResult Logout(int userId)
    {
        jwtTokensRepository.Remove(userId);
        return Ok();
    }

    [HttpGet]
    public string RefreshToken()
    {
        var userId = HttpContext.ExtractUserIdFromClaims();

        if (userId is null)
        {
            throw new InvalidOperationException();
        }
        var user = Users.FirstOrDefault(user => user.Id == userId);
        if (user is null)
        {
            throw new ArgumentException(nameof(userId));
        }
        var newToken = GenerateAndStoreToken(user);
        return newToken;
    }

    [HttpGet("allusers")]
    public List<string> GetAllUsers()
        => Users.Select(u => u.Login).ToList();
    
    
    private string GenerateAndStoreToken(User user)
    {
        var token = jwtTokenGenerator.GenerateToken(user);
        jwtTokensRepository.Update(user.Id, token);

        return token;
    }
}
