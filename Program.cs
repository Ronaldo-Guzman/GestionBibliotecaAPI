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


app.MapGet("/api/libros/", () =>
{
	return "Lista de libros";
});

app.MapGet("/api/libros/{id}", (int id) =>
{
	return $"Buscando libro con id: {id}";
});

app.MapPost("/api/libros", (LibroRequest libro) =>
{
	return $"Guardando libro con id: {libro.Id}";
});

app.MapPut("/api/libros/{id}", (int id, LibroRequest libro) =>
{
	return $"Modificando libro con id: {id}";
});

app.MapDelete("/api/libros/{id}", (int id) =>
{
	return $"Eliminando libro con id: {id}";
});



app.MapGet("/api/prestamos/", () =>
{
	return "Lista de préstamos";
});

app.MapGet("/api/prestamos/{id}", (int id) =>
{
	return $"Buscando préstamo con id: {id}";
});

app.MapPost("/api/prestamos", (PrestamoRequest prestamo) =>
{
	return $"Guardando préstamo con id: {prestamo.IdPrestamo}";
});

app.MapPut("/api/prestamos/{id}", (int id, PrestamoRequest prestamo) =>
{
	return $"Modificando préstamo con id: {id}";
});

app.MapDelete("/api/prestamos/{id}", (int id) =>
{
	return $"Eliminando préstamo con id: {id}";
});

app.Run();
