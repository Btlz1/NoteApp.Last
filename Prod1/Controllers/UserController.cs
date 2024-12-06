using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Models;

namespace btlz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, IMapper mapper)=>
        (_userRepository, _mapper) = (userRepository, mapper);

    [HttpGet]
    public ActionResult<ListOfUsers> GetUsers()
    {
        var users = _userRepository.GetUsers();
        
        return Ok(_mapper.Map<ListOfUsers>(users));
    }

    [HttpGet("{login}")]
    public ActionResult<UserVm> GetUser(string login)
    {
        var user = _userRepository.GetUserBy(user => user.Login == login.Trim());
        if (user is null)
        {
            return NotFound(login);
        }
        return Ok(_mapper.Map<UserVm>(user));
    }
    
    [HttpPost]
    public ActionResult<int> AddUser(CreateUserDto dto)
    {
        var newUser = _mapper.Map<User>(dto);
       
        var userId = _userRepository.AddUser(newUser);
        
        return Ok(userId);
    }
    
    [HttpPut("{id}")]
    public ActionResult UpdateUser(int id, UpdateUserDto dto)
    {
        var updatedUser = _mapper.Map<User>((id, dto));
       
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