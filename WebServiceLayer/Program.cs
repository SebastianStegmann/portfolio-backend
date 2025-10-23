using DataServiceLayer;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add JWT
var key = Encoding.ASCII.GetBytes("your-32-char-secret-key-here"); // Use secure key in prod
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
      {
      options.TokenValidationParameters = new TokenValidationParameters
      {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(key),
      ValidateIssuer = false,
      ValidateAudience = false
      };
      });

builder.Services.AddAuthorization();
// End JWT

builder.Configuration.AddJsonFile("config.json");

var connectionString = builder.Configuration.GetSection("ConnectionString").Value;

builder.Services.AddDbContext<ImdbContext>(options =>
    options.UseNpgsql(connectionString));

Console.WriteLine(connectionString);

builder.Services.AddControllers();

builder.Services.AddScoped<TitleDataService>();
builder.Services.AddScoped<NameDataService>();
builder.Services.AddScoped<PersonDataService>();

//jwt
builder.Services.AddAuthorization();
//end jwt

var app = builder.Build();

// JWT
app.UseAuthentication();
app.UseAuthorization();
// end JWT
//
app.MapControllers();

app.Run();
