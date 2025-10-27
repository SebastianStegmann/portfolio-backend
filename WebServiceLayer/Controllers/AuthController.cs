using DataServiceLayer;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using DataServiceLayer.Models;

namespace WebServiceLayer;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IConfiguration _config;
  private readonly ImdbContext _context;
  private const int SaltSize = 16;
  private const int HashSize = 32;
  private const int DegreeOfParallelism = 8;
  private const int Iterations = 4;
  private const int MemorySize = 1024 * 1024;

  public AuthController(IConfiguration config, ImdbContext context)
  {
    _config = config;
    _context = context;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginModel model) {
    var person = await _context.Persons.FirstOrDefaultAsync(p => p.Email == model.Email);
    if (person != null && VerifyPassword(model.Password, person.Password)) {
      var token = GenerateJwtToken(person.Id.ToString());
      return Ok(new { Token = token });
    }
    return Unauthorized();
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterModel model) {
    var existing = await _context.Persons.FirstOrDefaultAsync(p => p.Email == model.Email);
    if (existing != null) return BadRequest("User exists");
    var person = new Person { Email = model.Email, Password = HashPassword(model.Password), Name = model.Email, CreatedAt = DateTime.UtcNow };
    _context.Persons.Add(person);
    await _context.SaveChangesAsync();
    return Ok();
  }

  private string HashPassword(string password) {
    byte[] salt = new byte[SaltSize];
    using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);
    byte[] hash = GetHash(password, salt);
    var combined = new byte[salt.Length + hash.Length];
    Array.Copy(salt, 0, combined, 0, salt.Length);
    Array.Copy(hash, 0, combined, salt.Length, hash.Length);
    return Convert.ToBase64String(combined);
  }

  private byte[] GetHash(string password, byte[] salt) {
    var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)) {
      Salt = salt,
      DegreeOfParallelism = DegreeOfParallelism,
      Iterations = Iterations,
      MemorySize = MemorySize
    };
    return argon2.GetBytes(HashSize);
  }

  private bool VerifyPassword(string password, string hashedPassword) {
    byte[] combined = Convert.FromBase64String(hashedPassword);
    byte[] salt = new byte[SaltSize];
    byte[] storedHash = new byte[HashSize];
    Array.Copy(combined, 0, salt, 0, SaltSize);
    Array.Copy(combined, SaltSize, storedHash, 0, HashSize);
    byte[] newHash = GetHash(password, salt);
    return CryptographicOperations.FixedTimeEquals(storedHash, newHash);
  }

  private string GenerateJwtToken(string userId) {
    var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
    var jwtKey = _config["JWT:Key"] ?? throw new InvalidOperationException("JWT Key missing");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: _config["JWT:Issuer"],
        audience: _config["JWT:Audience"],
        claims: claims,
        expires: DateTime.Now.AddDays(7),
        signingCredentials: creds
        );
    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
