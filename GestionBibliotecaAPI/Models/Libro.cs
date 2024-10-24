using System;
using System.Collections.Generic;

namespace GestionBibliotecaAPI.Models;

public partial class Libro
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public int IdAutor { get; set; }

    public byte Estado { get; set; }

    public virtual Autor IdAutorNavigation { get; set; } = null!;

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
