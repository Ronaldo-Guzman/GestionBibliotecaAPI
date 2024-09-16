using GestionBibliotecaAPI.DTOs;

namespace GestionBibliotecaAPI.Services.Autores
{
	public interface IAutoresServices
	{
		Task<int> PostAutor(AutorRequest autor);
		Task<List<AutorResponse>> GetAutores();
		Task<AutorResponse> GetAutor(int autorId);
		Task<int> PutAutor(int autorId, AutorRequest autor);
		Task<int> DeleteAutor(int autorId);
	}
}
