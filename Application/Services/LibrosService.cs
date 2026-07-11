using Application.Commom;
using Application.DTO_s.EntityDTO.Libros;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LibrosService : ILibrosService
    {
        private readonly IMapper _mapper;
        private ILibrosRepository _librosRepository;
        private IAutoresRepository _autoresRepository;
        public LibrosService(ILibrosRepository librosRepository, IAutoresRepository autoresRepository, IMapper mapper)
        {
            _librosRepository  = librosRepository;
            _autoresRepository = autoresRepository;
            _mapper = mapper;
        }

        public async Task<List<LibrosDTO>>  GetLibrosPostedbefore2000()
        {
            return _mapper.Map<List<LibrosDTO>>(await _librosRepository.GetLibrosPostedbefore2000());
        }

        public async Task<SaveLibroDTO> AddLibro (SaveLibroDTO saveLibroDTO)
        {
            var autorExist = await _autoresRepository.GetById(saveLibroDTO.Autor_id);
            if (autorExist == null)
                throw new APIExceptions("El Autor no Existe", 404, "ERROR_GUARDANDO_LIBRO");

          
            await _librosRepository.Add(_mapper.Map<Libros>(saveLibroDTO));
            return saveLibroDTO;
        }
    }
}
