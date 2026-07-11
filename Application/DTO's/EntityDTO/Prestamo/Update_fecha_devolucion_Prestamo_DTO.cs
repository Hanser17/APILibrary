

using System.ComponentModel.DataAnnotations;

namespace Application.DTO_s.EntityDTO.Prestamo
{
    public class Update_fecha_devolucion_Prestamo_DTO
    {
        [Required(ErrorMessage = "El identificador del préstamo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe proporcionar un identificador de préstamo válido.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de devolución es obligatoria.")]
        public DateTime fecha_devolucion { get; set; }
    }
}
