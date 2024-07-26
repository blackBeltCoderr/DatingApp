using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(DataContext dataContext) : BaseApiController
{
    //private readonly DataContext _dataContext = dataContext;

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await dataContext.Users.ToListAsync();
        
        return Ok(users);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await dataContext.Users.FindAsync(id);

        if(user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}