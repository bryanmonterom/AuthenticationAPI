using AuthenticationService.DataAccess;
using AuthenticationService.Messages.Request;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService, IConfiguration _configuration, ILogger<UserDataAccess> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }


    [HttpPost("register", Name = "RegisterUser")]
    public async Task<IActionResult> Register(UserRequest request)
    {
        var result = await _userService.CreateUserAsync(request);
        if (result.Success) return Ok(result);

        return BadRequest(result.ErrorMessage);
    }


    [HttpPost("login", Name = "LoginUser")]
    public async Task<IActionResult> Login(UserRequest request)
    {
        var result = await _userService.LoginUser(request);
        if (result.Success) return Ok(result);

        return BadRequest(result.ErrorMessage);
    }
}