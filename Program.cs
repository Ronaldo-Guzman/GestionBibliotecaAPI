using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Endpoints;
using GestionBibliotecaAPI.Models;
using GestionBibliotecaAPI.Services.Libros;
using GestionBibliotecaAPI.Services.Usuarios;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BibliotecadbContext>(
    o=>o.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaDbConnection"))
 );

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<ILibrosServices, LibrosServices>();
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
		IssuerSigingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
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
