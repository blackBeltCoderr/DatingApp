using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
public class AccountController(DataContext dataContext, ITokenService tokenService) : BaseApiController
{
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDTo registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username already exists");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        dataContext.Users.Add(user);

        await dataContext.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };

    }
    
    [HttpPost]
    [Route("login")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

        if (user == null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    } 
    private async Task<bool> UserExists(string username)
    {
        return await dataContext.Users.AnyAsync(user => user.UserName.ToLower() == username.ToLower());
    }
}