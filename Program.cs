using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BibliotecadbContext>(
    o=>o.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaDbConnection"))
 );

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/autores/", () =>
{
    return "Lista de autores";
});

app.MapGet("/api/autores/{id}", (int id) =>
{
    return $"Buscando autores con id: {id}";
});

app.MapPost("/api/autores", (AutorRequest autor) =>
{
    return $"Guardando autor con usuarioId: {autor.Id}";
});

app.MapPut("/api/autores/{id}", (int id, AutorRequest autor) =>
{
	return $"Modificando autor con id: {id}";
});

app.MapDelete("/api/autores/{id}", (int id) =>
{
	return $"Eliminando autor con id: {id}";
});
app.Run();
