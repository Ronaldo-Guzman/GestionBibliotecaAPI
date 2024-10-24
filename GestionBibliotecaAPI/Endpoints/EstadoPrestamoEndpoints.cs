using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Services.EstadoPrestamos;
using Microsoft.OpenApi.Models;

namespace GestionBibliotecaAPI.Edpoints
{
    public static class EstadoPrestamoEndpoints
    {
        public static async void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/estado-prestamos").WithTags("EstadoPrestamos");

            group.MapGet("/", async (IEstadoPrestamosServices estadoPrestamosServices) =>
            {
                var estados = await estadoPrestamosServices.GetEstadoPrestamos();

                return Results.Ok(estados);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Estados de Préstamo",
                Description = "Muestra una lista de todos los estados de préstamo."
            });

            group.MapGet("/{id}", async (int id, IEstadoPrestamosServices estadoPrestamosServices) =>
            {
                var estado = await estadoPrestamosServices.GetEstadoPrestamo(id);
                if (estado == null)
                    return Results.NotFound();
                else
                    return Results.Ok(estado);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Estado de Préstamo",
                Description = "Buscar un estado de préstamo por id."
            });

            group.MapPost("/", async (EstadoPrestamoRequest estadoPrestamo, IEstadoPrestamosServices estadoPrestamosServices) =>
            {
                if (estadoPrestamo == null)
                    return Results.BadRequest();
                var id = await estadoPrestamosServices.PostEstadoPrestamo(estadoPrestamo);

                return Results.Created($"api/estado-prestamos/{id}", estadoPrestamo);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo Estado de Préstamo",
                Description = "Crear un nuevo estado de préstamo."
            });

            group.MapPut("/{id}", async (int id, EstadoPrestamoRequest estadoPrestamo, IEstadoPrestamosServices estadoPrestamosServices) =>
            {
                var result = await estadoPrestamosServices.PutEstadoPrestamo(id, estadoPrestamo);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.Ok(result);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar Estado de Préstamo",
                Description = "Actualiza un estado de préstamo existente."
            });

            group.MapDelete("/{id}", async (int id, IEstadoPrestamosServices estadoPrestamosServices) =>
            {
                var result = await estadoPrestamosServices.DeleteEstadoPrestamo(id);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.NoContent();
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar Estado de Préstamo",
                Description = "Eliminar un estado de préstamo existente."
            });
        }
    }
}
