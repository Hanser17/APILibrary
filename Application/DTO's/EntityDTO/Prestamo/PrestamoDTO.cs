using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s.EntityDTO.Prestamo
{
    public class PrestamoDTO
    {
        public int Libro_id { get; set; }
        public DateTime Fecha_prestamo { get; set; }
        public DateTime? Fecha_devolucion { get; set; }
    }
}
