using CodeTypeConnector.Security;
using CodeTypeConnector.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Register the SQLite security database.
builder.Services.AddDbContext<SecurityDbContext>(options =>
options.UseSqlite(
builder.Configuration.GetConnectionString("SecurityDatabase")));

// Register the security token service.
builder.Services.AddSingleton<SecurityTokenService>();

// Register the security token store.
builder.Services.AddScoped<SecurityTokenStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();