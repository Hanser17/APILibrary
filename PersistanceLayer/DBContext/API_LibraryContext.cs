using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace PersistanceLayer.DBContext
{
    public class API_LibraryContext : DbContext
    {
        public API_LibraryContext(DbContextOptions<API_LibraryContext> options)
            : base(options) {}

        public DbSet<Autores> Autores { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Autores>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.Nacionalidad).IsRequired();
            });
            modelBuilder.Entity<Libros>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired();
                entity.Property(e => e.Año_publicacion).IsRequired();
                entity.Property(e => e.Genero).IsRequired();
                entity.HasOne(x => x.Autor)
                      .WithMany(x => x.Libros)
                      .HasForeignKey(e => e.Autor_id)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Prestamos>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Fecha_prestamo).IsRequired();
                entity.Property(e => e.Fecha_devolucion);
                entity.HasOne(x => x.Libro)
                      .WithMany(x => x.Prestamos)
                      .HasForeignKey(x => x.Libro_id)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
