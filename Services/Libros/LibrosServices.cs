using AutoMapper;
using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionBibliotecaAPI.Services.Libros
{
	public class LibrosServices : ILibrosServices
	{
		private readonly BibliotecadbContext _db;
		private readonly  IMapper _mapper;

		public LibrosServices(BibliotecadbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<int> DeleteLibro(int libroId)
		{
			var libro = await _db.Libros.FindAsync(libroId);
			if (libro == null)
				return -1;

			_db.Libros.Remove(libro);

			return await _db.SaveChangesAsync();
		}

		public async Task<LibroResponse> GetLibro(int libroId)
		{
			var libro = await _db.Libros.FindAsync(libroId);
			var libroResponse = _mapper.Map<Libro, LibroResponse>(libro);

			return libroResponse;


		}

		public async Task<List<LibroResponse>> GetLibros()
		{
			var libros = await _db.Libros.ToListAsync();
			var libroList = _mapper.Map<List<Libro>, List<LibroResponse>>(libros);

			return libroList;
		}

		public async Task<int> PostLibro(LibroRequest libro)
		{
			var libroRequest = _mapper.Map<LibroRequest, Libro>(libro);
			await _db.Libros.AddAsync(libroRequest);

			return await _db.SaveChangesAsync();
		}

		public async Task<int> PutLibro(int libroId, LibroRequest libro)
		{
			var entity = await _db.Libros.FindAsync(libroId);
			if (entity == null)
				return -1;

			entity.Titulo = libro.Titulo;
			entity.IdAutor = libro.IdAutor;
			entity.Estado = libro.Estado;
			entity.IdAutorNavigation = entity.IdAutorNavigation;
			entity.Prestamos = entity.Prestamos;

			_db.Libros.Update(entity);

			return await _db.SaveChangesAsync();

		}
	}
}
