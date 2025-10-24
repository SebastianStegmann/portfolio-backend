using DataServiceLayer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline

app.MapControllers();

app.Run();


