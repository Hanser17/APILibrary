using Application.DTO_s.EntityDTO.Libros;

namespace Application.Interfaces
{
    public interface ILibrosService
    {
        Task<List<LibrosDTO>> GetLibrosPostedbefore2000();

        Task<SaveLibroDTO> AddLibro(SaveLibroDTO saveLibroDTO);
    }
}
