using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Endpoints;
using GestionBibliotecaAPI.Models;
using GestionBibliotecaAPI.Services.Libros;
using GestionBibliotecaAPI.Services.Usuarios;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using GestionBibliotecaAPI.Services.Autores;
using GestionBibliotecaAPI.Services.EstadoPrestamos;
using GestionBibliotecaAPI.Services.Prestamos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme 
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Ingrese el token JWT en el siguiente formato: Bearer {token}"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});

builder.Services.AddDbContext<BibliotecadbContext>(
    o=>o.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaDbConnection"))
 );

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IAutoresServices, AutoresServices>();
builder.Services.AddScoped<IEstadoPrestamosServices, EstadoPrestamosServices>();
builder.Services.AddScoped<ILibrosServices, LibrosServices>();
builder.Services.AddScoped<IPrestamosServices, PrestamosServices>();
builder.Services.AddScoped<IUsuarioServices, UsuarioServices>();

var jwtSettings = builder.Configuration.GetSection("JwtSetting");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(
options => {
	//Esquema por defecto
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
options => {
	//Permite usar HTTP en lugar de HTTPS
	options.RequireHttpsMetadata = false;
	//Guarda el token en el contexto de autenticacion
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
        ValidateIssuer = true,
		ValidateAudience = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
		ValidAudience = jwtSettings.GetValue<string>("Audience"),
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
	};
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.useEndpoints();

app.Run();


public partial class Program { }