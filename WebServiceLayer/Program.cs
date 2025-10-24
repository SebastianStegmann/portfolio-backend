using DataServiceLayer;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("config.json");

var connectionString = builder.Configuration.GetSection("ConnectionString").Value;

builder.Services.AddDbContext<ImdbContext>(options =>
    options.UseNpgsql(connectionString));

Console.WriteLine(connectionString);

builder.Services.AddControllers();

builder.Services.AddScoped<TitleDataService>();
builder.Services.AddScoped<NameDataService>();
builder.Services.AddScoped<PersonDataService>();


var app = builder.Build();

app.MapControllers();

app.Run();


