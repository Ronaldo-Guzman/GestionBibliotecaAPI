using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Models;

namespace GestionBibliotecaAPI.Services.EstadoPrestamos
{
	public class EstadoPrestamosServices : IEstadoPrestamosServices
	{
		private readonly BibliotecadbContext _db;
		private readonly IMapper _mapper;

		public EstadoPrestamosServices(BibliotecadbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<int> DeleteEstadoPrestamo(int estadoPrestamoId)
		{
			var estadoPrestamo = await _db.EstadoPrestamos.FindAsync(estadoPrestamoId);
			if (estadoPrestamo == null)
				return -1;

			_db.EstadoPrestamos.Remove(estadoPrestamo);

			return await _db.SaveChangesAsync();
		}

		public async Task<EstadoPrestamoResponse> GetEstadoPrestamo(int estadoPrestamoId)
		{
			var estadoPrestamo = await _db.EstadoPrestamos.FindAsync(estadoPrestamoId);
			var estadoPrestamoResponse = _mapper.Map<EstadoPrestamo, EstadoPrestamoResponse>(estadoPrestamo);

			return estadoPrestamoResponse;
		}
		public async Task<List<EstadoPrestamoRequest>> GetEstadoPrestamos()
		{
			var estadoPrestamos = await _db.EstadoPrestamos.ToListAsync();
			var estadoPrestamosList = _mapper.Map<List<EstadoPrestamo>, List<EstadoPrestamoResponse>>(estadoPrestamos);

			return estadoPrestamosList;
		}
		public async Task<int> PostEstadoPrestamos(EstadoPrestamoRequest estadoPrestamos)
		{
			var estadoPrestamoRequest = _mapper.Map<EstadoPrestamoRequest, EstadoPrestamo>(estadoPrestamo);
			await _db.EstadoPrestamos.AddAsync(estadoPrestamoRequest);

			return await _db.SaveChangesAsync();
		}
		public async Task<int> PutEstadoPrestamo(int estadoPrestamoId, EstadoPrestamoRequest estadoPrestamos)
		{
			var entity = await _db.EstadoPrestamos.FindAsync(estadoPrestamoId);
			if (entity == null)
				return -1;

			entity.Nombre = estadoPrestamos.NombreEstado;

			_db.Libros.Update(entity);

			return await _db.SaveChangesAsync();

		}
	}
}
