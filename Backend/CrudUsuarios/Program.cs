using CrudUsuarios.Data;
using CrudUsuarios.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// String de conexão com o SQL Server, lida de appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<UsuarioService>();

// CORS liberado para qualquer origem apenas para fins de desenvolvimento/estudo.
// Em um ambiente de produção, deve-se restringir aos domínios conhecidos do front-end.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

// Geração nativa de OpenAPI do .NET (gera o JSON em /openapi/v1.json)
builder.Services.AddOpenApi();

var app = builder.Build();

// Swagger UI habilitado apenas em ambiente de desenvolvimento,
// consumindo o JSON gerado pelo AddOpenApi acima
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "CrudUsuarios API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseCors("Frontend");

app.MapControllers();

app.Run();