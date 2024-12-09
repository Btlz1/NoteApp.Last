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
    
    public UserController(IUserRepository userRepository, IMapper mapper) : base(mapper)
        => _userRepository = userRepository;

    [HttpGet]
    public ActionResult<ListOfUsers> GetUsers()
    {
        var users = _userRepository.GetUsers();
        return Ok(Mapper.Map<ListOfUsers>(users));
    }

    [HttpGet("{id}")]
    public ActionResult<UserVm> GetUser(int id)
    {
        var user = _userRepository.GetUserById(id);
        if (user is null)
        {
            return NotFound(id);
        }
        return Ok(Mapper.Map<UserVm>(user));
    }
    
    [HttpPost]
    public ActionResult<int> AddUser(CreateUserDto dto)
    {
        var newUser = Mapper.Map<User>(dto);
        var userId = _userRepository.AddUser(newUser);
        return Ok(userId);
    }
    
    [HttpPut("{id}")]
    public ActionResult UpdateUser(int id, UpdateUserDto dto)
    {
        var updatedUser = Mapper.Map<User>((id, dto));
        _userRepository.UpdateUser(updatedUser);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        _userRepository.DeleteUser(id);
        return NoContent();
    }
    
    
}