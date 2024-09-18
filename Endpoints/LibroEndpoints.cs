using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Services.Libros;
using Microsoft.OpenApi.Models;

namespace GestionBibliotecaAPI.Edpoints
{
	public static class LibroEndpoints
	{
		public static async void Add(this IEndpointRouteBuilder routes)
		{
			var group = routes.MapGroup("/api/libros").WithTags("Libros");

			group.MapGet("/", async (ILibrosServices librosServices) =>
			{
				var libros = await librosServices.GetLibros();

				return Results.Ok(libros);
			}).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Obtener Libros",
				Description = "Muestra una lista de todos los libros."
			});

			group.MapGet("/{id}", async (int id, ILibrosServices librosServices) =>
			{
				var libro = await librosServices.GetLibro(id);
				if (libro == null)
					return Results.NotFound();
				else
					return Results.Ok(libro);
			}).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Obtener nuevo Libros",
				Description = "Buscar un libro por id."
			});

			group.MapPost("/", async (LibroRequest libro, ILibrosServices librosServices) =>
			{
				if (libro == null)
					return Results.BadRequest();
				var id = await librosServices.PostLibro(libro);

				return Results.Created($"api/libros/{id}", libro);
			}).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Crear nuevo Libro",
				Description = "Crear un nuevo producto."
			});

			group.MapPut("/{id}", async (int id, LibroRequest libro, ILibrosServices librosServices) =>
			{
				

				var result = await librosServices.PutLibro(id, libro);
				if (result == -1)
					return Results.NotFound();
				else
					return Results.Ok(result);
			}).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Modificar Libros",
				Description = "Actualiza un nuevo libro existente."
			});

			group.MapDelete("/{id}", async (int id, LibroRequest libro, ILibrosServices librosServices) =>
			{
			
				var result = await librosServices.DeleteLibro(id);
				if (result == -1)
					return Results.NotFound();
				else
					return Results.NoContent();
			}).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Eliminar Libros",
				Description = "Eliminar un nuevo libro existente."
			});
		}
	}
}
