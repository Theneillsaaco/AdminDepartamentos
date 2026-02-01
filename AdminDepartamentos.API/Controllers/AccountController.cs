using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdminDepartamentos.API.Models.LoginModels;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AdminDepartamentos.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[IgnoreAntiforgeryToken]
public class AccountController : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        _logger.LogInformation("Register User - Start.");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Register User - Model is not valid.");
            return BadRequest(ModelState);
        }

        if (model.Password != model.ConfirmPassword)
        {
            _logger.LogWarning("Register User - Passwords do not match.");
            return BadRequest("Las contraseñas no coinciden.");
        }

        var userExists = await _userManager.FindByEmailAsync(model.Email);

        if (userExists != null)
        {
            _logger.LogWarning("Register User - User already exists.");
            return BadRequest("El usuario ya existe.");
        }

        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Register User - Error creating user. Error: {@Errors}.",
                result.Errors.Select(e => e.Description));
            return BadRequest("Error creating user.");
        }

        _logger.LogInformation("Register User - User created successfully.");
        return Ok(new { Message = "Usuario creado exitosamente." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        _logger.LogInformation("Login User - Start.");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Login User - ModelState invalid.");
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            _logger.LogWarning("Login User - User not found. Email: {Email}.", model.Email);
            return Unauthorized("Usuario o contraseña incorrectos.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Login User - Invalid credentials. Email: {Email}.", model.Email);
            return Unauthorized("Usuario o contraseña incorrectos.");
        }

        var token = GenerateJwtToken(user);

        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.Now.AddMinutes(30)
        });

        _logger.LogInformation("Login User - User logged in successfully. Email: {Email}.", model.Email);
        return Ok(new { Message = "Login exitoso." });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("jwt");
        _logger.LogInformation("Logout User - User logged out successfully.");
        return Ok(new { Message = "Cierre de sesion con exito." });
    }

    [HttpGet("isauthenticated")]
    public IActionResult IsAuthenticated()
    {
        if (!User.Identity.IsAuthenticated)
        {
            _logger.LogInformation("IsAuthenticated - User not authenticated.");
            return Unauthorized(new { IsAuthenticated = false });
        }

        _logger.LogInformation("IsAuthenticated - User authenticated. User: {UserName}.", User.Identity.Name);
        return Ok(new { IsAuthenticated = true, User = User.Identity.Name });
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpGet("csrf-token")]
    public IActionResult GetCsrToken([FromServices] IAntiforgery antiforgery)
    {
        var tokens = antiforgery.GetAndStoreTokens(HttpContext);

        _logger.LogInformation("GetCsrfToken - CSRF token generated and stored in cookie.");
        return Ok(new { token = tokens.RequestToken });
    }

    #region Fields

    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        IConfiguration configuration, ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
    }

    #endregion
}