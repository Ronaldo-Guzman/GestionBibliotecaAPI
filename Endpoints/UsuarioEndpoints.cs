using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Services.Usuarios;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace GestionBibliotecaAPI.Endpoints
{
    public static class UsuarioEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes) {
            var group = routes.MapGroup("/api/usuarios").WithTags("Usuarios");

            group.MapGet("/", async (IUsuarioServices usuarioServices) =>
            {
                var usuarios = await usuarioServices.GetUsuarios();

                return Results.Ok(usuarios);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Usuarios",
                Description = "Muestra una lista de todos los usuarios."
            });

            group.MapGet("/{id}", async (int id, IUsuarioServices usuarioServices) =>
            {
                var usuario = await usuarioServices.GetUsuario(id);
                if (usuario == null)
                    return Results.NotFound();
                else
                    return Results.Ok(usuario);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Usuario",
                Description = "Buscar un usuario por id."
            });

            group.MapPost("/", async (UsuarioRequest usuario, IUsuarioServices usuarioServices) =>
            {
                if (usuario == null)
                    return Results.BadRequest();
                var id = await usuarioServices.PostUsuario(usuario);

                return Results.Created($"api/usuarios/{id}", usuario);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo Usuario",
                Description = "Crear un nuevo Usuario."
            });

            group.MapPut("/{id}", async (int id, UsuarioRequest usuario, IUsuarioServices usuarioServices) =>
            {
                var result = await usuarioServices.PutUsuario(id, usuario);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.Ok(result);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar Usuario",
                Description = "Actualiza un usuario existente."
            });

            group.MapDelete("/{id}", async (int id, IUsuarioServices usuarioServices) =>
            {

                var result = await usuarioServices.DeleteUsuario(id);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.NoContent();
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar Usuario",
                Description = "Eliminar un usuario existente."
            });

            group.MapPost("/login", async (UsuarioRequest usuario, IUsuarioServices usuarioServices, IConfiguration config) =>
            {
                var login = await usuarioServices.Login(usuario);

                if (login is null)
                    return Results.Unauthorized();
                else
                {
                    var jwtSettings = config.GetSection("jwtSetting");
                    var secretkey = jwtSettings.GetValue<string>("Secretkey");
                    var issuer = jwtSettings.GetValue<string>("Issuer");
                    var audience = jwtSettings.GetValue<string>("Audience");

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(secretkey);


                    var tokenDescriptor = new SecurityTokenDescriptor

                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new  Claim(ClaimTypes.Name, usuario.NombreUsuario),
						   new Claim(ClaimTypes.Role, usuario.IdRol.ToString())


						}),
                        Expires = DateTime.UtcNow.AddHours(1),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)

                    };

                    //Crear token, usando parametros definidos
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    //Convertit el token a una cadena 
                    var jwt = tokenHandler.WriteToken(token);

                    return Results.Ok(jwt);
                }
            }).WithOpenApi(o => new OpenApiOperation(o)
			{
				Summary = "Login Usuario",
				Description = "Generar token para inicio de sesion."
			});
		}

    }
}
