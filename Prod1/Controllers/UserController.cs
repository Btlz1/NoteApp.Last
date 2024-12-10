using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Models;

namespace btlz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseController
{
    private readonly IUserRepository _userRepository;
    
    public UserController(IUserRepository userRepository)
        => _userRepository = userRepository;

    [HttpGet]
    public ActionResult<UsersVm> GetUsers()
        => Ok(_userRepository.GetUsers());

    [HttpGet("{id}")]
    public ActionResult<UsersVm> GetUserById(int id)
        => Ok(_userRepository.GetUserById(id));


    [HttpPost]
    public ActionResult<int> AddUser(CreateUserDto dto)
        => Ok(_userRepository.AddUser(dto));


    [HttpPut("{id}")]
    public ActionResult UpdateUser(int id, UpdateUserDto dto)
        => Ok(_userRepository.UpdateUser(id, dto));
    
    
    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        _userRepository.DeleteUser(id);
        return NoContent();
    }
}