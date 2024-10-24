using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Services.Prestamos;
using Microsoft.OpenApi.Models;

namespace GestionBibliotecaAPI.Edpoints
{
	public static class PrestamoEdpoints
	{
		public static async void Add(this IEndpointRouteBuilder routes)
		{
			var group = routes.MapGroup("/api/prestamos").WithTags("Prestamos");

			group.MapGet("/", async (IPrestamosServices prestamosServices) =>
			{
				var prestamos = await prestamosServices.GetPrestamos();

				return Results.Ok(prestamos);
			}).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Obtener Prestamos",
				Description = "Muestra una lista de todos los prestamos."
			});

			group.MapGet("/{id}", async (int id, IPrestamosServices prestamosServices) =>
			{
				var prestamo = await prestamosServices.GetPrestamo(id);
				if (prestamo == null)
					return Results.NotFound();
				else
					return Results.Ok(prestamo);
			}).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Obtener Prestamo",
				Description = "Buscar un prestamo por id."
			});

			group.MapPost("/", async (PrestamoRequest prestamo, IPrestamosServices prestamosServices) =>
			{
				if (prestamo == null)
					return Results.BadRequest();
				var id = await prestamosServices.PostPrestamo(prestamo);

				return Results.Created($"api/prestamos/{id}", prestamo);
			}).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Crear nuevo Prestamo",
				Description = "Crear un nuevo prestamo."
			});

			group.MapPut("/{id}", async (int id, PrestamoRequest prestamo, IPrestamosServices prestamosServices) =>
			{
				var result = await prestamosServices.PutPrestamo(id, prestamo);
				if (result == -1)
					return Results.NotFound();
				else
					return Results.Ok(result);
			}).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Modificar Prestamo",
				Description = "Actualiza un prestamo existente."
			});

			group.MapDelete("/{id}", async (int id, IPrestamosServices prestamosServices) =>
			{
				var result = await prestamosServices.DeletePrestamo(id);
				if (result == -1)
					return Results.NotFound();
				else
					return Results.NoContent();
			}).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Eliminar Prestamo",
				Description = "Eliminar un prestamo existente."
			});
		}
	}
}
