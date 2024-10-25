using GestionBibliotecaAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestionBibliotecaAPI.IntegrationTest
{
    [TestClass]
    public class LibroEndpointsTest
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
        public async Task ObtenerLibros_ConTokenValido_RetornaListaDeLibros()
        {
            // Arrange: Pasar autorización a la cabecera
            AgregarTokenAlaCabecera();

            // Act: Realizar solicitud para obtener los libros
            var libros = await _httpClient.GetFromJsonAsync<List<LibroResponse>>("api/libros");

            // Assert: Verificar que la lista de libros no sea nula y que tenga elementos
            Assert.IsNotNull(libros, "La lista de libros no debería ser nula.");
            Assert.IsTrue(libros.Count > 0, "La lista de libros debería contener al menos un elemento.");
        }

        [TestMethod]
        public async Task ObtenerLibroPorId_LibroExistente_RetornaLibro()
        {
            // Arrange: Pasar autorización a la cabecera y establecer ID de libro existente
            AgregarTokenAlaCabecera();
            var IdLibro = 1; // Cambiarlo a un ID existente en la base de datos

            // Act: Realizar solicitud para obtener libro por ID
            var libro = await _httpClient.GetFromJsonAsync<LibroResponse>($"api/libros/{IdLibro}");

            // Assert: Verificar que el libro no sea nulo y que tenga el ID correcto
            Assert.IsNotNull(libro, "El libro no debería ser nulo.");
            Assert.AreEqual(IdLibro, libro.Id, "El ID del libro devuelto no coincide.");
        }

        [TestMethod]
        public async Task GuardarLibro_ConDatosValidos_RetornaCreated()
        {
            // Arrange: Pasar autorización a la cabecera y preparar el nuevo libro
            AgregarTokenAlaCabecera();
            var newLibro = new LibroRequest
            {
                Titulo = "El principito",
                IdAutor = 2,
                Estado = 1
            };

            // Act: Realizar solicitud para guardar el libro
            var response = await _httpClient.PostAsJsonAsync("api/libros", newLibro);

            // Assert: Verificar que el estado sea Created
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "El libro no se creó correctamente.");
        }

        [TestMethod]
        public async Task GuardarLibro_ConTituloDuplicado_RetornaConflict()
        {
            // Arrange: Pasar autorización a la cabecera y preparar un libro con título duplicado
            AgregarTokenAlaCabecera();
            var duplicatedLibro = new LibroRequest
            {
                Titulo = "Cien años de soledad",
                IdAutor = 4,
                Estado = 1
            };

            // Act: Realizar solicitud para guardar el libro
            var response = await _httpClient.PostAsJsonAsync("api/libros", duplicatedLibro);

            // Assert: Verificar que el estado sea Conflict (409)
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode, "Se debería haber devuelto un conflicto por título duplicado.");
        }

        [TestMethod]
        public async Task ModificarLibro_LibroExistente_RetornaOk()
        {
            // Arrange: Pasar autorización a la cabecera y preparar los datos modificados
            AgregarTokenAlaCabecera();
            var existingLibro = new LibroRequest
            {
                Titulo = "Cien años de soledad",
                IdAutor = 4,
                Estado = 1
            };
            var IdLibro = 1;

            // Act: Realizar solicitud para modificar el libro
            var response = await _httpClient.PutAsJsonAsync($"api/libros/{IdLibro}", existingLibro);

            // Assert: Verificar que el estado sea OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "El libro no se modificó correctamente.");
        }

        [TestMethod]
        public async Task EliminarLibro_LibroExistente_RetornaNoContent()
        {
            // Arrange: Pasar autorización a la cabecera y establecer ID del libro a eliminar
            AgregarTokenAlaCabecera();
            var IdLibro = 17; // Cambiar por el ID de un libro existente

            // Act: Realizar solicitud para eliminar el libro
            var response = await _httpClient.DeleteAsync($"api/libros/{IdLibro}");

            // Assert: Verificar que el estado sea NoContent
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "El libro no se eliminó correctamente.");
        }

        [TestMethod]
        public async Task EliminarLibro_LibroNoExistente_RetornaNotFound()
        {
            // Arrange: Pasar autorización a la cabecera y establecer un ID de libro no existente
            AgregarTokenAlaCabecera();
            var IdLibro = 999; // ID que no existe en la base de datos

            // Act: Realizar solicitud para eliminar un libro no existente
            var response = await _httpClient.DeleteAsync($"api/libros/{IdLibro}");

            // Assert: Verificar que el estado sea NotFound
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Se debería haber devuelto NotFound al intentar eliminar un libro no existente.");
        }
    }
}
