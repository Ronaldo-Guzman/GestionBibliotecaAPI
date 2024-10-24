namespace GestionBibliotecaAPI.DTOs
{
    public class AutorResponse
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Nacionalidad { get; set; } = null!;

        public DateOnly FechaNacimiento { get; set; }

        public byte Estado { get; set; }
    }
    public class AutorRequest
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Nacionalidad { get; set; } = null!;

        public DateOnly FechaNacimiento { get; set; }

        public byte Estado { get; set; }
    }
}
