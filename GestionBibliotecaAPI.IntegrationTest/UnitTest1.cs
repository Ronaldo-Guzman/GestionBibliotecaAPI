using GestionBibliotecaAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;

namespace GestionBibliotecaAPI.IntegrationTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            // Crear instancia de la aplicación en memoria
            using var application = new WebApplicationFactory<Program>();

            // Crear cliente HTTP para enviar solicitudes
            using var _httpClient = application.CreateClient();

            var usuarioSession = new UsuarioRequest { NombreUsuario = "Ronaldo", Contraseña = "123" };
            var response = await _httpClient.PostAsJsonAsync("api/usuarios/login", usuarioSession);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<string>();
            }
        }
    }
}