using AutoMapper;
using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionBibliotecaAPI.Services.Autores
{
	public class AutoresServices : IAutoresServices
	{
		private readonly BibliotecadbContext _db;
		private readonly IMapper _mapper;

		public AutoresServices(BibliotecadbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<int> DeleteAutor(int autoreId)
		{
			var autor = await _db.Autores.FindAsync(autoreId);
			if (autor == null)
				return -1;

			_db.Autores.Remove(autor);

			return await _db.SaveChangesAsync();
		}

		public async Task<AutorResponse> GetAutor(int autorId)
		{
			var autor = await _db.Autores.FindAsync(autorId);
			var autorResponse = _mapper.Map<Autor, AutorResponse>(autor);

			return autorResponse;


		}

		public async Task<List<AutorResponse>> GetAutores()
		{
			var autores = await _db.Autores.ToListAsync();
			var autorList = _mapper.Map<List<Autor>, List<AutorResponse>>(autores);

			return autorList;
		}

		public async Task<int> PostAutor(AutorRequest autor)
		{
			var autorRequest = _mapper.Map<AutorRequest, Autor>(autor);
			await _db.Autores.AddAsync(autorRequest);

			return await _db.SaveChangesAsync();
		}

		public async Task<int> PutAutor(int autorId, AutorRequest autor)
		{
			var entity = await _db.Autores.FindAsync(autorId);
			if (entity == null)
				return -1;

			entity.Nombre = autor.Nombre;
			entity.Nacionalidad = autor.Nacionalidad;
			entity.FechaNacimiento = autor.FechaNacimiento;
			entity.Libros = entity.Libros;

			_db.Autores.Update(entity);

			return await _db.SaveChangesAsync();

		}
	}
}
