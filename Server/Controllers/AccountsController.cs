using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using BlazorCrud.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlazorCRUD.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AccountsController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("Create")]
    public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
    {
        var user = new IdentityUser { UserName = model.Email, Email = model.Email};
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (result.Succeeded)
        {
            return BuildToken(model);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
    {
        userInfo.Name = userInfo.Email;
        var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false,
            lockoutOnFailure: false);        
        
        if (result.Succeeded)
        {
            return Ok(BuildToken(userInfo));
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return BadRequest(ModelState);
    }

    private UserToken BuildToken(UserInfo userInfo)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
            new Claim(ClaimTypes.Name, userInfo.Name),
            new Claim(ClaimTypes.Email, userInfo.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Expiration time of token
        var expiration = DateTime.UtcNow.AddHours(1);

        var token = new JwtSecurityToken(
            null,
            null,
            claims,
            expires: expiration,
            signingCredentials: creds);

        return new UserToken
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}