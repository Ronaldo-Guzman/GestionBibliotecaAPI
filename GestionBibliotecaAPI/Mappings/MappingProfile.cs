using AutoMapper;
using GestionBibliotecaAPI.DTOs;
using GestionBibliotecaAPI.Models;

namespace GestionBibliotecaAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // Modelo -> DTO
            CreateMap<Libro, LibroResponse>();
            CreateMap<Autor, AutorResponse>();
            CreateMap<Usuario, UsuarioResponse>();
            CreateMap<Prestamo, PrestamoResponse>();
            CreateMap<EstadoPrestamo, EstadoPrestamoResponse>();

            // DTO -> Modelo
            CreateMap<LibroRequest, Libro>();
            CreateMap<AutorRequest, Autor>();
            CreateMap<UsuarioRequest, Usuario>();
            CreateMap<PrestamoRequest, Prestamo>();
            CreateMap<EstadoPrestamoRequest, EstadoPrestamo>();
        }
    }
}
