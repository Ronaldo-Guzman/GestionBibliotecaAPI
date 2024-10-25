using GestionBibliotecaAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestionBibliotecaAPI.IntegrationTest
{
    [TestClass]
    public class AutorEndpointsTest
    {
        private static HttpClient _httpClient;
        private static WebApplicationFactory<Program> _factory;
        private static string _token;

        ///<summary>
        /// Configurar entorno de prueba inicializando la API y obteniendo el token JWT
        ///</summary>
        [ClassInitialize]
        public static async Task ClassInit(TestContext context)
        {
            // Crear la instancia de la aplicación en memoria
            _factory = new WebApplicationFactory<Program>();

            // Crear el cliente HTTP
            _httpClient = _factory.CreateClient();

            // Arrange: Preparar la carga útil para el inicio de sesión (similar a los usuarios)
            var loginRequest = new UsuarioRequest { NombreUsuario = "Ronaldo", Contraseña = "123" };

            // Act: Enviar la solicitud de inicio de sesión
            var loginResponse = await _httpClient.PostAsJsonAsync("api/usuarios/login", loginRequest);

            // Assert: Verificar que el inicio de sesión sea exitoso
            loginResponse.EnsureSuccessStatusCode();
            _token = (await loginResponse.Content.ReadAsStringAsync()).Trim('"');
        }

        ///<summary>
        /// Agregar token de autorización a la cabecera del cliente HTTP
        ///</summary>
        [TestInitialize]
        public void AgregarTokenAlaCabecera()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        [TestMethod]
        public async Task ObtenerAutores_ConTokenValido_RetornaListaDeAutores()
        {
            // Arrange: Pasar autorización a la cabecera
            AgregarTokenAlaCabecera();

            // Act: Realizar solicitud para obtener los autores
            var autores = await _httpClient.GetFromJsonAsync<List<AutorResponse>>("api/autores");

            // Assert: Verificar que la lista de autores no sea nula y que tenga elementos
            Assert.IsNotNull(autores, "La lista de autores no debería ser nula.");
            Assert.IsTrue(autores.Count > 0, "La lista de autores debería contener al menos un elemento.");
        }

        [TestMethod]
        public async Task ObtenerAutorPorId_AutorExistente_RetornaAutor()
        {
            // Arrange: Pasar autorización a la cabecera y establecer ID de autor existente
            AgregarTokenAlaCabecera();
            var IdAutor = 1; // Cambiarlo a un ID existente en la base de datos

            // Act: Realizar solicitud para obtener autor por ID
            var autor = await _httpClient.GetFromJsonAsync<AutorResponse>($"api/autores/{IdAutor}");

            // Assert: Verificar que el autor no sea nulo y que tenga el ID correcto
            Assert.IsNotNull(autor, "El autor no debería ser nulo.");
            Assert.AreEqual(IdAutor, autor.Id, "El ID del autor devuelto no coincide.");
        }

        [TestMethod]
        public async Task GuardarAutor_ConDatosValidos_RetornaCreated()
        {
            // Arrange: Pasar autorización a la cabecera y preparar el nuevo autor
            AgregarTokenAlaCabecera();
            var newAutor = new AutorRequest
            {
                Nombre = "Stiven R",
                Nacionalidad = "Colombiana",
                FechaNacimiento = new DateOnly(1927, 3, 6),
                Estado = 1
            };

            // Act: Realizar solicitud para guardar el autor
            var response = await _httpClient.PostAsJsonAsync("api/autores", newAutor);

            // Assert: Verificar que el estado sea Created
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "El autor no se creó correctamente.");
        }

        [TestMethod]
        public async Task GuardarAutor_ConNombreDuplicado_RetornaConflict()
        {
            // Arrange: Pasar autorización a la cabecera y preparar un autor con nombre duplicado
            AgregarTokenAlaCabecera();
            var duplicatedAutor = new AutorRequest
            {
                Nombre = "Ronaldo GM", 
                Nacionalidad = "Salvadoreña",
                FechaNacimiento = new DateOnly(1927, 3, 6),
                Estado = 1
            };

            // Act: Realizar solicitud para guardar el autor
            var response = await _httpClient.PostAsJsonAsync("api/autores", duplicatedAutor);

            // Assert: Verificar que el estado sea Conflict (409)
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode, "Se debería haber devuelto un conflicto por nombre duplicado.");
        }

        [TestMethod]
        public async Task ModificarAutor_AutorExistente_RetornaOk()
        {
            // Arrange: Pasar autorización a la cabecera y preparar los datos modificados
            AgregarTokenAlaCabecera();
            var existingAutor = new AutorRequest
            {
                Nombre = "Alexander GM",
                Nacionalidad = "Estadounidense",
                FechaNacimiento = new DateOnly(1927, 3, 6),
                Estado = 1
            };
            var IdAutor = 1; 

            // Act: Realizar solicitud para modificar el autor
            var response = await _httpClient.PutAsJsonAsync($"api/autores/{IdAutor}", existingAutor);

            // Assert: Verificar que el estado sea OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "El autor no se modificó correctamente.");
        }

        [TestMethod]
        public async Task EliminarAutor_AutorExistente_RetornaNoContent()
        {
            // Arrange: Pasar autorización a la cabecera y establecer ID del autor a eliminar
            AgregarTokenAlaCabecera();
            var IdAutor = 17; // Cambiar por el ID de un autor existente

            // Act: Realizar solicitud para eliminar el autor
            var response = await _httpClient.DeleteAsync($"api/autores/{IdAutor}");

            // Assert: Verificar que el estado sea NoContent
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "El autor no se eliminó correctamente.");
        }

        [TestMethod]
        public async Task EliminarAutor_AutorNoExistente_RetornaNotFound()
        {
            // Arrange: Pasar autorización a la cabecera y establecer un ID de autor no existente
            AgregarTokenAlaCabecera();
            var IdAutor = 999; // ID que no existe en la base de datos

            // Act: Realizar solicitud para eliminar un autor no existente
            var response = await _httpClient.DeleteAsync($"api/autores/{IdAutor}");

            // Assert: Verificar que el estado sea NotFound
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Se debería haber devuelto NotFound al intentar eliminar un autor no existente.");
        }
    }
}
