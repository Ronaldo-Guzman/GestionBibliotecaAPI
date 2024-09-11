using System;
using System.Collections.Generic;

namespace GestionBibliotecaAPI.Models;

public partial class Prestamo
{
    public int IdPrestamo { get; set; }

    public int IdUsuario { get; set; }

    public int IdLibro { get; set; }

    public DateOnly FechaPrestamo { get; set; }

    public DateOnly FechaDevolucion { get; set; }

    public int IdEstadoPrestamo { get; set; }

    public virtual EstadoPrestamo IdEstadoPrestamoNavigation { get; set; } = null!;

    public virtual Libro IdLibroNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
