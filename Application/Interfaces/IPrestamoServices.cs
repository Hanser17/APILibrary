

using Application.DTO_s.EntityDTO.Libros;
using Application.DTO_s.EntityDTO.Prestamo;

namespace Application.Interfaces
{
    public interface IPrestamoServices
    {
        Task<List<LibrosNoDevueltosDTO>> GetLibrosNoDevueltos();

        Task<bool> BorrarPrestamo(int id);

        Task<PrestamoDTO> Update_fecha_devolucion_Prestamo (Update_fecha_devolucion_Prestamo_DTO Update_fecha_devolucion_Prestamo_DTO);

    }
}
