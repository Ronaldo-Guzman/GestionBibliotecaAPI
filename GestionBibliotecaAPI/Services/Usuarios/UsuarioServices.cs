using AutoMapper;
using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionBibliotecaAPI.Services.Usuarios
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly BibliotecadbContext _db;
        private readonly IMapper _mapper;

        public UsuarioServices(BibliotecadbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> DeleteUsuario(int usuarioId)
        {
            var usuario = await _db.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return -1;

            _db.Usuarios.Remove(usuario);

            return await _db.SaveChangesAsync();
        }

        public async Task<UsuarioResponse> GetUsuario(int usuarioId)
        {
            var usuario = await _db.Usuarios.FindAsync(usuarioId);
            var usuarioResponse = _mapper.Map<Usuario, UsuarioResponse>(usuario);

            return usuarioResponse;
        }

        public async Task<List<UsuarioResponse>> GetUsuarios()
        {
            var usuarios = await _db.Usuarios.ToListAsync();
            var usuariosList = _mapper.Map<List<Usuario>, List<UsuarioResponse>>(usuarios);

            return usuariosList;
        }

        public async Task<UsuarioResponse> Login(UsuarioRequest usuario)
        {
            var usuarioEntity = await _db.Usuarios.FirstOrDefaultAsync(
                o => o.NombreUsuario == usuario.NombreUsuario
                && o.Contraseña == usuario.Contraseña
                );

            var usuarioResponse = _mapper.Map<Usuario, UsuarioResponse>(usuarioEntity);

            return usuarioResponse;
        }
        public async Task<int> PostUsuario(UsuarioRequest usuario)
        {
            var entity = _mapper.Map<UsuarioRequest, Usuario>(usuario);
            await _db.Usuarios.AddAsync(entity);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> PutUsuario(int usuarioId, UsuarioRequest usuario)
        {
            var entity = await _db.Usuarios.FindAsync(usuarioId);
            if (entity == null)
                return -1;

            entity.NombreUsuario = usuario.NombreUsuario;
            entity.Contraseña = usuario.Contraseña;
            entity.IdRolNavigation = entity.IdRolNavigation;
            entity.Prestamos = entity.Prestamos;

            _db.Usuarios.Update(entity);
            return await _db.SaveChangesAsync();
        }
    }
}
