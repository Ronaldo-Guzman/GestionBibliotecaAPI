﻿using GestionBibliotecaAPI.DTOs;

namespace GestionBibliotecaAPI.Services.Usuarios
{
    public interface IUsuarioServices
    {
        Task<int> PostUsuario(UsuarioRequest usuario);
        Task<List<UsuarioResponse>> GetUsuarios();
        Task<UsuarioResponse> GetUsuario(int usuarioId);
        Task<int> PutUsuario(int usuarioId, UsuarioRequest usuario);
        Task<int> DeleteUsuario(int usuarioId);
        Task<UsuarioResponse> Login(UsuarioRequest usuario);
    }
}
