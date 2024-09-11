namespace GestionBibliotecaAPI.DTOs
{
    public class RolesUsuarioResponse
    {
        public int IdRol { get; set; }

        public string NombreRol { get; set; } = null!;
    }
    public class RolesUsuarioRequest
    {
        public int IdRol { get; set; }

        public string NombreRol { get; set; } = null!;
    }
}
