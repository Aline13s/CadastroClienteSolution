using AutoMapper;
using CadastroCliente.Application.Dtos;
using CadastroCliente.Application.Interfaces;
using CadastroCliente.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repo;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriaDto>> ListarAsync()
        {
            var categorias = await _repo.GetAllAsync();
            return categorias.Select(_mapper.Map<CategoriaDto>);
        }
    }
}
