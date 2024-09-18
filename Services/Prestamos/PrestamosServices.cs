using AutoMapper;
using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionBibliotecaAPI.Services.Prestamos
{
	public class PrestamosServices : IPrestamosServices
	{
		private readonly BibliotecadbContext _db;
		private readonly IMapper _mapper;

		public PrestamosServices(BibliotecadbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<int> DeletePrestamo(int prestamoId)
		{
			var prestamo = await _db.Prestamos.FindAsync(prestamoId);
			if (prestamo == null)
				return -1;

			_db.Prestamos.Remove(prestamo);
			return await _db.SaveChangesAsync();
		}

		public async Task<List<PrestamoResponse>> GetPrestamos()
		{
			var prestamos = await _db.Prestamos.ToListAsync();
			var prestamoList = _mapper.Map<List<Prestamo>, List<PrestamoResponse>>(prestamos);

			return prestamoList;
		}

		public async Task<PrestamoResponse> GetPrestamo(int prestamoId)
		{
			var prestamo = await _db.Prestamos.FindAsync(prestamoId);

			var prestamoResponse = _mapper.Map<Prestamo, PrestamoResponse>(prestamo);
			return prestamoResponse;
		}

		public async Task<int> PostPrestamo(PrestamoRequest prestamo)
		{
			var prestamoEntity = _mapper.Map<PrestamoRequest, Prestamo>(prestamo);
			await _db.Prestamos.AddAsync(prestamoEntity);

			return await _db.SaveChangesAsync();
		}

		public async Task<int> PutPrestamo(int prestamoId, PrestamoRequest prestamo)
		{
			var entity = await _db.Prestamos.FindAsync(prestamoId);
			if (entity == null)
				return -1;

			// Asignando las propiedades de la tabla Prestamo
			entity.IdUsuario = prestamo.IdUsuario;
			entity.IdLibro = prestamo.IdLibro;
			entity.FechaPrestamo = prestamo.FechaPrestamo;
			entity.FechaDevolucion = prestamo.FechaDevolucion;
			entity.IdEstadoPrestamo = prestamo.IdEstadoPrestamo;
			entity.IdEstadoPrestamoNavigation = entity.IdEstadoPrestamoNavigation;
			entity.IdLibroNavigation = entity.IdLibroNavigation;
			entity.IdUsuarioNavigation = entity.IdUsuarioNavigation;

			_db.Prestamos.Update(entity);

			return await _db.SaveChangesAsync();
		}
	}
}
