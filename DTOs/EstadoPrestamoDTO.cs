namespace GestionBibliotecaAPI.DTOs
{
    public class EstadoPrestamoResponse
    {
        public int IdEstadoPrestamo { get; set; }

        public string NombreEstado { get; set; } = null!;
    }
    public class EstadoPrestamoRequest
    {
        public int IdEstadoPrestamo { get; set; }

        public string NombreEstado { get; set; } = null!;
    }
}
