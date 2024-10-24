using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Services.Autores;
using Microsoft.OpenApi.Models;

namespace GestionBibliotecaAPI.Edpoints
{
    public static class AutorEndpoints
    {
        public static async void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/autores").WithTags("Autores");

            group.MapGet("/", async (IAutoresServices autoresServices) =>
            {
                var autores = await autoresServices.GetAutores();

                return Results.Ok(autores);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Autores",
                Description = "Muestra una lista de todos los autores."
            });

            group.MapGet("/{id}", async (int id, IAutoresServices autoresServices) =>
            {
                var autor = await autoresServices.GetAutor(id);
                if (autor == null)
                    return Results.NotFound();
                else
                    return Results.Ok(autor);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Autor",
                Description = "Buscar un autor por id."
            });

            group.MapPost("/", async (AutorRequest autor, IAutoresServices autoresServices) =>
            {
                if (autor == null)
                    return Results.BadRequest();
                var id = await autoresServices.PostAutor(autor);

                return Results.Created($"api/autores/{id}", autor);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo Autor",
                Description = "Crear un nuevo autor."
            });

            group.MapPut("/{id}", async (int id, AutorRequest autor, IAutoresServices autoresServices) =>
            {
                var result = await autoresServices.PutAutor(id, autor);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.Ok(result);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar Autor",
                Description = "Actualiza un autor existente."
            });

            group.MapDelete("/{id}", async (int id, IAutoresServices autoresServices) =>
            {
                var result = await autoresServices.DeleteAutor(id);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.NoContent();
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar Autor",
                Description = "Eliminar un autor existente."
            });
        }
    }
}
