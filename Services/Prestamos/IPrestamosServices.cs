using GestionBibliotecaAPI.DTOs;

namespace GestionBibliotecaAPI.Services.Prestamos
{
	public interface IPrestamosServices
	{
		Task<int> PostPrestamo(PrestamoRequest prestamo);
		Task<List<PrestamoResponse>> GetPrestamos();
		Task<PrestamoResponse> GetPrestamos(int prestamoId);
		Task<int> PutPrestamo(int PrestamoId, PrestamoRequest prestamo);
		Task<int> DeletePrestamo(int PrestamoId);
	}
}
