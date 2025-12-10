using DataServiceLayer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new() {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

builder.Configuration.AddJsonFile("config.json");

var connectionString = builder.Configuration.GetSection("ConnectionString").Value;

builder.Services.AddDbContext<ImdbContext>(options =>
    options.UseNpgsql(connectionString));


Console.WriteLine(connectionString);

builder.Services.AddMapster();

builder.Services.AddControllers();


builder.Services.AddScoped<TitleDataService>();
builder.Services.AddScoped<NameDataService>();
builder.Services.AddScoped<PersonDataService>();
builder.Services.AddScoped<FunctionsDataService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
