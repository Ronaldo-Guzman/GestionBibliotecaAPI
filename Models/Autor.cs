using System;
using System.Collections.Generic;

namespace GestionBibliotecaAPI.Models;

public partial class Autor
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Nacionalidad { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public byte Estado { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
