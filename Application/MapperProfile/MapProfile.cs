using Application.DTO_s.EntityDTO.Libros;
using Application.DTO_s.EntityDTO.Prestamo;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MapperProfile
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Libros, LibrosDTO>()
               .ForMember(dest => dest.Libro_id,
               opt => opt.MapFrom(src => src.Id))
              .ReverseMap();

            CreateMap<Libros, SaveLibroDTO>()
               .ReverseMap();

            CreateMap<Prestamos, PrestamoDTO>()
               .ReverseMap();
        }
    }
}
