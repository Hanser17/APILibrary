using Application.Commom;
using Application.DTO_s.EntityDTO.Libros;
using Application.DTO_s.EntityDTO.Prestamo;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Services
{
    public class PrestamoServices : IPrestamoServices
    {
        private readonly IPrestamosRepository _prestamosRepository;
        private readonly IMapper _mapper;
        public PrestamoServices(IPrestamosRepository prestamosRepository, IMapper mapper)
        {
            _prestamosRepository = prestamosRepository;
            _mapper = mapper;
        }

        public async Task<bool> BorrarPrestamo(int id)
        {
            var prestamoExist = await _prestamosRepository.GetById(id);
            if (prestamoExist == null)
                throw new APIExceptions("El Prfestamo no Existe", 404, "ERROR_AL_BORRAR_PRESTAMO");
            await _prestamosRepository.Delete(prestamoExist);

            return true;
        }

        public async  Task<List<LibrosNoDevueltosDTO>> GetLibrosNoDevueltos()
        {
            var prestamos = await _prestamosRepository.GetPrestamosPendientesAsync();
            return prestamos.Select(p => new LibrosNoDevueltosDTO
            {
                autor_id = p?.Libro?.Autor_id,
                nombre = p?.Libro?.Autor?.Nombre,
                libro_id = p.Libro_id,
                titulo = p?.Libro?.Titulo
            }).ToList();
        }

        public async Task<PrestamoDTO> Update_fecha_devolucion_Prestamo
            (Update_fecha_devolucion_Prestamo_DTO Update_fecha_devolucion_Prestamo_DTO)
        {
            var prestamoExist = await _prestamosRepository.GetById(Update_fecha_devolucion_Prestamo_DTO.Id);
            if (prestamoExist == null)
                throw new APIExceptions("El Prfestamo no Existe", 404, "ERROR_AL_ACTUALIZAR_PRESTAMO");

            prestamoExist.Fecha_devolucion = Update_fecha_devolucion_Prestamo_DTO.fecha_devolucion;
            await _prestamosRepository.Update(prestamoExist);

            return _mapper.Map<PrestamoDTO>(prestamoExist);

        }
    }
}
