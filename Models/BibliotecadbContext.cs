using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace GestionBibliotecaAPI.Models;

public partial class BibliotecadbContext : DbContext
{
    public BibliotecadbContext()
    {
    }

    public BibliotecadbContext(DbContextOptions<BibliotecadbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autore> Autores { get; set; }

    public virtual DbSet<EstadoPrestamo> EstadoPrestamos { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<RolesUsuario> RolesUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-NFDMETJ\\SQLEXPRESS; Database=bibliotecadb; User Id=sa; Password=132; Encrypt=False; TrustServerCertificate=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__autores__3213E83F47CB81C1");

            entity.ToTable("autores");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nacionalidad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadoPrestamo>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPrestamo).HasName("PK__estado_p__F498A882BB15880A");

            entity.ToTable("estado_prestamo");

            entity.Property(e => e.IdEstadoPrestamo).HasColumnName("idEstadoPrestamo");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombreEstado");
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__libros__3213E83FD735B303");

            entity.ToTable("libros");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdAutor).HasColumnName("id_autor");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdAutorNavigation).WithMany(p => p.Libros)
                .HasForeignKey(d => d.IdAutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__libros__id_autor__398D8EEE");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.IdPrestamo).HasName("PK__Prestamo__A4876C1309E28EEE");

            entity.ToTable("Prestamo");

            entity.Property(e => e.IdPrestamo).HasColumnName("idPrestamo");
            entity.Property(e => e.FechaDevolucion).HasColumnName("fechaDevolucion");
            entity.Property(e => e.FechaPrestamo).HasColumnName("fechaPrestamo");
            entity.Property(e => e.IdEstadoPrestamo).HasColumnName("idEstadoPrestamo");
            entity.Property(e => e.IdLibro).HasColumnName("idLibro");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

            entity.HasOne(d => d.IdEstadoPrestamoNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdEstadoPrestamo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prestamo__idEsta__44FF419A");

            entity.HasOne(d => d.IdLibroNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdLibro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prestamo__idLibr__440B1D61");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prestamo__idUsua__4316F928");
        });

        modelBuilder.Entity<RolesUsuario>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__roles_us__3C872F7673D3298A");

            entity.ToTable("roles_usuario");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombreRol");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuarios__4E3E04ADB8D614D3");

            entity.ToTable("usuarios");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("contraseña");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nombre_usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuarios__idRol__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
