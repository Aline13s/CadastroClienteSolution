using AutoMapper;
using CadastroCliente.Application.Dtos;
using CadastroCliente.Application.Interfaces;
using CadastroCliente.Domain.Entities;
using CadastroCliente.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<int> CriarAsync(ClienteDto dto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(dto);
                return await _repo.CreateAsync(cliente);
            }
            catch (SqlException ex) when (ex.Number == 547) // FK violation
            {
                throw new InvalidOperationException("A categoria informada não existe.");
            }
        }

        public async Task AtualizarAsync(ClienteDto dto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(dto);
                await _repo.UpdateAsync(cliente);
            }
            catch (SqlException ex) when (ex.Number == 547) // FK violation
            {
                throw new InvalidOperationException("A categoria informada não existe.");
            }
        }

        public async Task ExcluirAsync(int id)
            => await _repo.DeleteAsync(id);

        public async Task<ClienteListarDto?> ObterPorIdAsync(int id)
        {
            var cliente = await _repo.GetByIdAsync(id);
            return cliente is null ? null : _mapper.Map<ClienteListarDto>(cliente);
        }

        public async Task<IEnumerable<ClienteListarDto>> ListarAsync(string? nome, int? categoriaId)
        {
            var clientes = await _repo.ListAsync(nome, categoriaId);
            return clientes.Select(_mapper.Map<ClienteListarDto>);
        }
    }
}
