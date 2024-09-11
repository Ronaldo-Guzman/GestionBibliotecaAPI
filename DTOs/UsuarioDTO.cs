namespace GestionBibliotecaAPI.DTOs
{
    public class UsuarioResponse
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; } = null!;

        public string Contraseña { get; set; } = null!;

        public int IdRol { get; set; }
    }
    public class UsuarioRequest
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; } = null!;

        public string Contraseña { get; set; } = null!;

        public int IdRol { get; set; }
    }
}
