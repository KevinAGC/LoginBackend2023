using LoginBackend2023;
using LoginBackend2023.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CuentasController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public CuentasController(
        UserManager<IdentityUser> userManager,
        IConfiguration config,
        SignInManager<IdentityUser> signInManager,
        ApplicationDbContext context
        )
    {
        _userManager = userManager;
        _config = config;
        _signInManager = signInManager;
        _context = context;
    }





    [HttpPost("registrar")]
    public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
    {
        var usuario = new IdentityUser
        {
            UserName = credencialesUsuario.Email,
            Email = credencialesUsuario.Email
        };
        var resultado = await _userManager.CreateAsync(usuario, credencialesUsuario.Password);
        if (resultado.Succeeded)
        {
            return await ConstruirToken(credencialesUsuario);
        }
        return BadRequest(resultado.Errors);
    }

    private async Task<ActionResult<RespuestaAutenticacion>> ConstruirToken(CredencialesUsuario credencialesUsuario)
    {
        var claims = new List<Claim>()
        {
            new Claim("email",credencialesUsuario.Email)
        };
        var usuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email);
        var claimsRoles = await _userManager.GetClaimsAsync(usuario);

        claims.AddRange(claims);

        var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["LlaveJWT"]));
        var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

        var expiracion = DateTime.UtcNow.AddDays(1);

        var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

        return new RespuestaAutenticacion
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expiracion = expiracion,
        };
    }

    [HttpGet("RenovarToken")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
    {

        var emailClaims = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();
        var credencialesUsuario = new CredencialesUsuario() { Email = emailClaims };
        Console.Write(emailClaims);
        return await ConstruirToken(credencialesUsuario);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
    {
        var resultado = await _signInManager.PasswordSignInAsync(
            credencialesUsuario.Email,
            credencialesUsuario.Password,
            isPersistent: false,
            lockoutOnFailure: false);
        if (resultado.Succeeded)
        {
            return await ConstruirToken(credencialesUsuario);
        }
        else
        {
            return BadRequest("Login Incorrecto");
        }
    }

    [HttpPost("Favorito")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Create(Favoritos favoritos)
    {
        try
        {
            var email = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();
            favoritos.UserId = _userManager.Users
                .Where(m => m.Email == email)
    .Select(m => m.Id)
    .SingleOrDefault();
            if (ModelState.IsValid)
            {
                _context.Add(favoritos);
                await _context.SaveChangesAsync();
                return Ok("Favorita Agregada");
            }
        }
        catch (Exception ex)
        {
            return BadRequest("Esta noticia ya esta en tus favoritos!");
        }

        return null;
    }

    [HttpGet("Favorito")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(string correo)
    {
        var userId = _userManager.Users
       .Where(m => m.Email == correo)
       .Select(m => m.Id)
       .SingleOrDefault();

        return Ok(userId);


    }
}