using UserStoreApi.Models;
using UserStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace UserStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController (UsersService usersService) =>
        _usersService = usersService;
    
    [HttpGet]
    [Authorize]
    public async Task<List<UserDTO>> Get() => 
        await _usersService.GetASync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<UserDTO>> Get(string id) 
    {
        var user = await _usersService.GetAsync(id);

        if (user is null) return NotFound();

        return user;
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDTO loginUser) 
    {
        var token = await _usersService.LoginAsync(loginUser);

        if (token is null) return NotFound();

        return token;
    }

    
    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _usersService.CreateAsync(newUser);

        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, User updatedUser)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null) return NotFound();

        updatedUser.Id = user.Id;

        await _usersService.UpdateAsync(id, updatedUser);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null) return NotFound();

        await _usersService.DeleteAsync(id);

        return NoContent();
    }
}