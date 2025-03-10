using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkipAuth.Models;
using SkipAuth.RequestsDTO;
using SkipAuth.ResponseDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SkipAuth.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly DatabaseContext _dbContext = new();

    [Route("register")]
    [HttpPost]
    public IActionResult Register(RegisterRequestDTO request)
    {
        if (request.Login.Length > 30 || request.Name.Length >= 30 || request.Login == "" || request.Name == "")
        {
            return BadRequest("The \"name\" and \"login\" fields length must be in [1,30]");
        }

        if (_dbContext.Users.FirstOrDefault(x => x.Login == request.Login) != null)
        {
            return BadRequest("User already exist");
        }

        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

        User user = new User
        {
            Id = new Guid(),
            Name = request.Name,
            Login = request.Login,
            Password = passwordHash,
            Role = "STUDENT"
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();


        var jwt = new JwtSecurityToken(
                issuer: "issuer",
                audience: "audience",
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")), SecurityAlgorithms.HmacSha256));

        TokenResponse response = new TokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            Expire = DateTime.UtcNow.Add(TimeSpan.FromMinutes(60))
        };

        Token token = new Token
        {
            UserId = _dbContext.Users.First(x => x.Login == request.Login).Id,
            Token1 = response.Token,
            ExpireAt = response.Expire
        };

        _dbContext.Tokens.Add(token);
        _dbContext.SaveChanges();

        return Ok(response);
    }

    [Route("login")]
    [HttpPost]
    public IActionResult Login(LoginRequestDTO request)
    {
        User? user = _dbContext.Users.AsNoTracking()
            .FirstOrDefault(x => x.Login == request.Login);

        if (user == null)
        {
            return BadRequest("Wrong pair login-password");
        }

        if (BCrypt.Net.BCrypt.Verify(request.Password, user.Password) == false)
        {
            return BadRequest("Wrong pair login-password");
        }

        var jwt = new JwtSecurityToken(
                issuer: "issuer",
                audience: "audience",
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")), SecurityAlgorithms.HmacSha256));

        TokenResponse response = new TokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            Expire = DateTime.UtcNow.Add(TimeSpan.FromMinutes(60))
        };

        Token token = new Token
        {
            Id = new Guid(),
            UserId = _dbContext.Users.First(x => x.Login == request.Login).Id,
            Token1 = response.Token,
            ExpireAt = response.Expire
        };

        _dbContext.Tokens.Add(token);
        _dbContext.SaveChanges();

        return Ok(response);
    }
}
