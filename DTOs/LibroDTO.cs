namespace GestionBibliotecaAPI.DTOs
{
    public class LibroResponse
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public int IdAutor { get; set; }

        public byte Estado { get; set; }
    }
    public class LibroRequest
    {
        //public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public int IdAutor { get; set; }

        public byte Estado { get; set; }
    }
}
