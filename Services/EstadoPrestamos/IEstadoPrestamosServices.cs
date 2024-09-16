using GestionBibliotecaAPI.DTOs;

namespace GestionBibliotecaAPI.Services.EstadoPrestamos
{
	public interface IEstadoPrestamosServices
	{
		Task<int> PostEstadoPrestamos(EstadoPrestamoRequest estadoPrestamo);
		Task<List<EstadoPrestamoRequest>> GetEstadoPrestamos();
		Task<EstadoPrestamoResponse> GetEstadoPrestamo(int estadoPrestamoId);
		Task<int> PutEstadoPrestamo(int estadoPrestamoId, EstadoPrestamoRequest estadoPrestamo);
		Task<int> DeleteEstadoPrestamo(int estadoPrestamoId);
	}
}
