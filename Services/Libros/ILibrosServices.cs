using GestionBibliotecaAPI.DTOs;

namespace GestionBibliotecaAPI.Services.Libros
{
	public interface ILibrosServices
	{
		Task<int> PostLibro(LibroRequest libro);
		Task<List<LibroResponse>> GetLibros();
		Task<LibroResponse> GetLibro(int libroId);
		Task<int> PutLibro(int libroId, LibroRequest libro);
		Task<int> DeleteLibro(int libroId);
	}
}
