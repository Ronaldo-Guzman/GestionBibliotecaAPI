using GestionBibliotecaAPI.DTOs;

namespace GestionBibliotecaAPI.Services.Autore
{
	public interface IAutoreServices
	{
		Task<int> PostAutore(AutoreRequest autore);
		Task<List<AutoreRequest>> GetAutores();
		Task<AutoreResponse> GetAutore(int autoreId);
		Task<int> PutAutore(int autoreId, AutoreRequest autore);
		Task<int> DeleteAutore(int autoreId);
	}
}
