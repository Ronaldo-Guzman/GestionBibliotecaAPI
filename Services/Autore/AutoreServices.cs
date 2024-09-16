using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Models;

namespace GestionBibliotecaAPI.Services.Autore
{
	public class AutoreServices : IAutoreServices
	{
		private readonly BibliotecadbContext _db;
		private readonly IMapper _mapper;

		public AutoreServices(BibliotecadbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<int> DeleteAutore(int autoreId)
		{
			var autore = await _db.Autores.FindAsync(autoreId);
			if (autore == null)
				return -1;

			_db.Autores.Remove(autore);

			return await _db.SaveChangesAsync();
		}

		public async Task<AutoreResponse> GetAutore(int autoreId)
		{
			var autore = await _db.Autores.FindAsync(autoreId);
			var autoreResponse = _mapper.Map<Autore, AutoreResponse>(autore);

			return libroResponse;


		}

		public async Task<List<AutoreRequest>> GetAutores()
		{
			var autores = await _db.Autores.ToListAsync();
			var autoreList = _mapper.Map<List<Autore>, List<AutoreResponse>>(autores);

			return autoreList;
		}

		public async Task<int> PostAutore(AutoreRequest autore)
		{
			var autoreRequest = _mapper.Map<AutoreRequest, Autore>(autore);
			await _db.Autores.AddAsync(autoreRequest);

			return await _db.SaveChangesAsync();
		}

		public async Task<int> PutAutore(int autoreId, AutoreRequest autore)
		{
			var entity = await _db.Autores.FindAsync(autoreId);
			if (entity == null)
				return -1;

			entity.Nombre = autore.Nombre;
			entity.Nacionalidad = autore.Nacionalidad;
			entity.FechaNacimiento = autore.FechaNacimiento;

			_db.Autores.Update(entity);

			return await _db.SaveChangesAsync();

		}
	}
}
