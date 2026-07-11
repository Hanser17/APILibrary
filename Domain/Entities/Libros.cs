

namespace Domain.Entities
{
    public class Libros  : BaseEntity
    {
        public string Titulo { get; set; }
        public int Autor_id { get; set; }
        public int Año_publicacion { get; set; }
        public string Genero { get; set; }

        public Autores Autor { get; set; } = null!;
        public ICollection<Prestamos> Prestamos { get; set; } = [];
    }
}
