using CrudUsuarios.Data;
using CrudUsuarios.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<UsuarioService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();