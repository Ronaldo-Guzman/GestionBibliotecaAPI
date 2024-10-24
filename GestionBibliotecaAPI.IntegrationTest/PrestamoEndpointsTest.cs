using GestionBibliotecaAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;

namespace GestionBibliotecaAPI.Tests
{
    [TestClass]
    public class PrestamoEndpointsTest
    {
        private readonly HttpClient _client;

        public PrestamoEndpointsTest()
        {
            // Configurar la aplicación para pruebas
            var application = new WebApplicationFactory<Program>();
            _client = application.CreateClient();
        }

        [TestMethod]
        public async Task ObtenerPrestamos_ConTokenValido_RetornaListaDePrestamos()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token_valido");

            // Act
            var response = await _client.GetAsync("/api/prestamos");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var prestamos = await response.Content.ReadFromJsonAsync<List<PrestamoResponse>>();
            Assert.IsNotNull(prestamos);
            Assert.IsTrue(prestamos.Count > 0);
        }

        [TestMethod]
        public async Task ObtenerPrestamoPorId_PrestamoExistente_RetornaPrestamo()
        {
            // Arrange
            int idPrestamo = 12;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token_valido");

            // Act
            var response = await _client.GetAsync($"/api/prestamos/{idPrestamo}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var prestamo = await response.Content.ReadFromJsonAsync<PrestamoResponse>();
            Assert.IsNotNull(prestamo);
            Assert.AreEqual(idPrestamo, prestamo.IdPrestamo);
        }

        [TestMethod]
        public async Task GuardarPrestamo_ConDatosValidos_RetornaCreated()
        {
            // Arrange
            var nuevoPrestamo = new PrestamoRequest
            {
                IdPrestamo = 12,
                IdUsuario = 2,
                IdLibro = 3,
                FechaPrestamo = DateOnly.FromDateTime(DateTime.Now),
                FechaDevolucion = DateOnly.FromDateTime(DateTime.Now.AddDays(14)),
                IdEstadoPrestamo = 1
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token_valido");

            // Act
            var response = await _client.PostAsJsonAsync("/api/prestamos", nuevoPrestamo);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var prestamoCreado = await response.Content.ReadFromJsonAsync<PrestamoResponse>();
            Assert.IsNotNull(prestamoCreado);
            Assert.AreEqual(nuevoPrestamo.IdPrestamo, prestamoCreado.IdPrestamo);
        }

        [TestMethod]
        public async Task ModificarPrestamo_PrestamoExistente_RetornaOk()
        {
            // Arrange
            int idPrestamo = 12;
            var prestamoModificado = new PrestamoRequest
            {
                IdPrestamo = idPrestamo,
                IdUsuario = 2,
                IdLibro = 4,
                FechaPrestamo = DateOnly.FromDateTime(DateTime.Now),
                FechaDevolucion = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
                IdEstadoPrestamo = 2
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token_valido");

            // Act
            var response = await _client.PutAsJsonAsync($"/api/prestamos/{idPrestamo}", prestamoModificado);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task EliminarPrestamo_PrestamoExistente_RetornaNoContent()
        {
            // Arrange
            int idPrestamo = 1;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token_valido");

            // Act
            var response = await _client.DeleteAsync($"/api/prestamos/{idPrestamo}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public async Task EliminarPrestamo_PrestamoNoExistente_RetornaNotFound()
        {
            // Arrange
            int idPrestamoInexistente = 99;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token_valido");

            // Act
            var response = await _client.DeleteAsync($"/api/prestamos/{idPrestamoInexistente}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task ObtenerPrestamoPorId_PrestamoNoExistente_RetornaNotFound()
        {
            // Arrange
            int idPrestamoInexistente = 99;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token_valido");

            // Act
            var response = await _client.GetAsync($"/api/prestamos/{idPrestamoInexistente}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
