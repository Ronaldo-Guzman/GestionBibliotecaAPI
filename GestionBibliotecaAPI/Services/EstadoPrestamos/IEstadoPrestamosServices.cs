using GestionBibliotecaAPI.DTOs;

namespace GestionBibliotecaAPI.Services.EstadoPrestamos
{
	public interface IEstadoPrestamosServices
	{
		Task<int> PostEstadoPrestamo(EstadoPrestamoRequest estadoPrestamo);
		Task<List<EstadoPrestamoResponse>> GetEstadoPrestamos();
		Task<EstadoPrestamoResponse> GetEstadoPrestamo(int estadoPrestamoId);
		Task<int> PutEstadoPrestamo(int estadoPrestamoId, EstadoPrestamoRequest estadoPrestamo);
		Task<int> DeleteEstadoPrestamo(int estadoPrestamoId);
	}
}
