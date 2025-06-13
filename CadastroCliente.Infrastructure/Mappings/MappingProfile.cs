using AutoMapper;
using CadastroCliente.Application.Dtos;
using CadastroCliente.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CadastroCliente.Infrastructure.Mappings
{
    /// <summary>
    /// Configura mapeamentos Entity ↔ DTO.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClienteDto, Cliente>().ForMember(dest => dest.Categoria, opt => opt.Ignore());
            CreateMap<Cliente, ClienteDto>().ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.Nome));
            CreateMap<ClienteListar, ClienteListarDto>().ReverseMap();
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<MenuCompleto, MenuCompletoDto>().ReverseMap();
        }
    }
}
