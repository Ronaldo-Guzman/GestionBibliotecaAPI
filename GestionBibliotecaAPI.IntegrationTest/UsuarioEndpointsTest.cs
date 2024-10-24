using GestionBibliotecaAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace GestionBibliotecaAPI.IntegrationTest
{
    [TestClass]
    public class UsuarioEndpointsTest
    {
        private static HttpClient _httpClient;
        private static WebApplicationFactory<Program> _factory;
        private static string _token;

        ///<summary>
        /// Configurar entorno de prueba inicializando la API y obteniendo el token JWT
        ///</summary>
        ///

        [ClassInitialize]
        public static async Task ClassInit(TestContext context)
        {
            // Crear la instancia de la aplicación en memoria
            _factory = new WebApplicationFactory<Program>();

            // Crear el cliente HTTP
            _httpClient = _factory.CreateClient();

            //Arrange: Preparar la carga util para el inicio de sesión
            var loginRequest = new UsuarioRequest { NombreUsuario = "Ronaldo", Contraseña = "123" };

            // Act: Enviar la solicitud de inicio de sesion
            var loginResponse = await _httpClient.PostAsJsonAsync("api/usuarios/login", loginRequest);

            //Assert: Verificar que el inicio de sesión sea exitoso
            loginResponse.EnsureSuccessStatusCode();
            _token = (await loginResponse.Content.ReadAsStringAsync()).Trim('"');
        }

        ///<summary>
        /// Agregar token de autorizacion a la cabecera del cliente HTTP
        ///</summary>
        [TestInitialize]
        public void AgregarTokenAlaCabecera()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        [TestMethod]
        public async Task ObtenerUsuarios_ConTokenValido_RetornaListaDeUsuarios()
        {
            // Arrange: Pasar autorizacion a la cabecera
            AgregarTokenAlaCabecera();
            // ACT: Realizar solicitud para obtener los usuarios
            var usuarios = await _httpClient.GetFromJsonAsync<List<UsuarioResponse>>("api/usuarios");
            // Assert: Verificar que la lista de usuarios no sea nula y que tenga elementos
            Assert.IsNotNull(usuarios, "La lista de usuarios no deberia ser nula.");
            Assert.IsTrue(usuarios.Count > 0, "La lista de usuarios deberia contener al menos un elemento");
        }

        [TestMethod]
        public async Task ObtenerUsuarioPorId_usuarioExistente_RetornaUsuario()
        {
            // Arrange: Pasar autorizacion a la cabecera y establecer ID de usuario existente
            AgregarTokenAlaCabecera();
            var IdUsuario = 18;
            // ACT: Realizar solicitud para obtener usuario por ID
            var usuario = await _httpClient.GetFromJsonAsync<UsuarioResponse>($"api/usuarios/{IdUsuario}");
            // Assert:  Verificar que el usuario sea nulo y que tenga el ID correcto
            Assert.IsNotNull(usuario, "El usuario no deberia ser nulo.");
            Assert.AreEqual(IdUsuario, usuario.IdUsuario, "El ID del usuario devuelto no coincide.");
        }

        [TestMethod]
        public async Task GuardarUsuario_ConDatosValidos_RetornaCreated()
        {
            //Arrage : Pasar autorizacion a la cabecera y preparar el nuevo usuario 
            AgregarTokenAlaCabecera();

            var newUsuario = new UsuarioRequest { NombreUsuario = "Alex", Contraseña = "123", IdRol = 1 };

            // ACT: Realizar solicitud para Guargar el usuario 

            var response = await _httpClient.PostAsJsonAsync("api/usuarios", newUsuario);

            //Assert Verifica el codigo de estado created 

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "El usuario no se creo correctamente");
        }
        [TestMethod]
        public async Task GuardarUsuario_UsernameDuplicado_RetornaConflict()
        {
            //Arrage : Pasar autorizacion a la cabecera y preparar el usuario duplicado
            AgregarTokenAlaCabecera();

            var newUsuario = new UsuarioRequest { NombreUsuario = "Ronaldo", Contraseña = "123", IdRol = 1 };

            // ACT: Realizar solicitud para Guargar el usuario con combre de usuario duplicado

            var response = await _httpClient.PostAsJsonAsync("api/usuarios", newUsuario);

            //Assert Verifica el codigo de estado Conflict 

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "Se esperaba un conflicto al intentar crear usuario duplicado");
        }

        [TestMethod]
        public async Task ModificarUsuario_UsuarioExistente_RetornaOk()
        {
            //Arrage : Pasar autorizacion a la cabecera y preparar el usuario modificado, pasando un ID
            AgregarTokenAlaCabecera();
            var existingUsuario = new UsuarioRequest { NombreUsuario = "Rauda", Contraseña = "1234", IdRol = 1 };
            var IdUsuario = 18;
            // ACT: Realizar solicitud para modificar usuario existente
            var response = await _httpClient.PutAsJsonAsync($"api/usuarios/{IdUsuario}", existingUsuario);
            //Assert Verifica que la respuesta sea OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "El usuario no se modificó correctamente");
        }

        [TestMethod]
        public async Task EliminarUsuario_UsuarioExistente_RetornaNoContent()
        {
            //Arrage : Pasar autorizacion a la cabecera, pasando un ID
            AgregarTokenAlaCabecera();
            var IdUsuario = 14;
            // ACT: Realizar solicitud para eliminar usuario existente
            var response = await _httpClient.DeleteAsync($"api/usuarios/{IdUsuario}");
            //Assert Verifica que la respuesta sea Noontent
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "El usuario no se eliminó correctamente");
        }


        [TestMethod]
        public async Task EliminarUsuario_UsuarioNoExistente_RetornaNoFound()
        {
            //Arrage : Pasar autorizacion a la cabecera, pasando un ID
            AgregarTokenAlaCabecera();
            var IdUsuario = 16;
            // ACT: Realizar solicitud para eliminar usuario existente
            var response = await _httpClient.DeleteAsync($"api/usuarios/{IdUsuario}");
            //Assert Verifica que la respuesta sea Noontent
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Se esperaba un 404 NotFound al intentar eliminar el usuario inexistente.");
        }
    }
}
