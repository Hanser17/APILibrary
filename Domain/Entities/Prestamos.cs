using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Prestamos : BaseEntity
    {
        public int Libro_id { get; set; }
        public DateTime Fecha_prestamo { get; set; }
        public DateTime? Fecha_devolucion { get; set; }
        public Libros Libro { get; set; }

    }
}
