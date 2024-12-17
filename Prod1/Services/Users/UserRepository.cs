using AutoMapper;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Database;
using btlz.Exceptions;
using btlz.Models;
using btlz.Settings;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace btlz.Services;

public class UserRepository : IUserRepository 
{
    private readonly btlzDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IJwtTokensRepository _jwtTokensRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    public UserRepository(btlzDbContext dbContext, IMapper mapper, 
        IJwtTokensRepository jwtTokensRepository, IJwtTokenGenerator tokenGenerator) 
        => (_dbContext, _mapper, _jwtTokensRepository, _tokenGenerator)
            = (dbContext, mapper, jwtTokensRepository, tokenGenerator);
    
    public async Task<UsersVm> GetUsers()
    {
        var listOfUsers = await _dbContext.Users
            .Select(user => new UserVm(user.Id, user.Login, user.Password))
            .ToListAsync();  // Используем ToListAsync для асинхронного выполнения

        var users = new UsersVm(listOfUsers);
        return users;
    }

    public async Task<UsersVm?> GetUserById(int id)
    {
        _ = TryGetUserByIdAndThrowIfNotFound(id);
        var listOfUsers = await _dbContext.Users
            .Where(user => user.Id == id)
            .Select(user => new UserVm(user.Id, user.Login, user.Password))
            .ToListAsync();
        
        var userById = new UsersVm(listOfUsers);
        return userById;
    }

    

    public async Task<User> AddUser(CreateUserDto dto)
    {
        if (await _dbContext.Users.AnyAsync(user => user.Login == dto.Login))
        {
            throw new ArgumentException(nameof(dto.Login));
        }
        var user = _mapper.Map<User>(dto);
        await _dbContext.Users.AddAsync(user); 
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<int> UpdateUser(int userId, UpdateUserDto dto) 
    {
        var user = TryGetUserByIdAndThrowIfNotFound(userId);
        
        var updatedUser = _mapper.Map<(int, UpdateUserDto), User>((userId, dto));
        user.Login = updatedUser.Login;
        user.Password = updatedUser.Password;
        await _dbContext.SaveChangesAsync();
        return user.Id;
    }
    
    public async Task DeleteUser(int id)
    {
        var user = TryGetUserByIdAndThrowIfNotFound(id);
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<User> LoginUser(string login,[FromBody] string password)
    {
        var user =await _dbContext.Users.FirstOrDefaultAsync(user => (user.Login == login));
        if (user is null)
        {
            throw new ArgumentException(nameof(login));
        }
        if (user.Password != password)
        {
            throw new ArgumentException(nameof(password));
        }
        return user;
    }

    public string Generate(User user)
    {
        var token = _tokenGenerator.GenerateToken(user);
        _jwtTokensRepository.Update(user.Id, token);
        return token;
    }
    public User TryGetUserByIdAndThrowIfNotFound(int id)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new UserNotFoundException(id);
        }
        return user;
    }
}