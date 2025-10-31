using DataServiceLayer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
// End JWT

builder.Configuration.AddJsonFile("config.json");

// Add services to the container

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


// jwt
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
