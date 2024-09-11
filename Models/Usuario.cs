using System;
using System.Collections.Generic;

namespace GestionBibliotecaAPI.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual RolesUsuario IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
