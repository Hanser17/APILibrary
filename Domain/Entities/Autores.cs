

namespace Domain.Entities
{
    public class Autores : BaseEntity
    {
        public string Nombre { get; set; }
        public string Nacionalidad { get; set; }

        public ICollection<Libros> Libros { get; set; } = [];
    }
}
