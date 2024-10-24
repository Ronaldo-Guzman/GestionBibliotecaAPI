﻿using System;
using System.Collections.Generic;

namespace GestionBibliotecaAPI.Models;

public partial class RolesUsuario
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}