using CadastroCliente.Application.Dtos;
using CadastroCliente.Application.Interfaces;
using CadastroCliente.Shared.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CadastroCliente.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;
        private readonly IValidator<ClienteDto> _validator;

        public ClientesController(IClienteService service, IValidator<ClienteDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteDto dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(ApiResponse<string>.Fail(validation.Errors.Select(e => e.ErrorMessage)));

            try
            {
                var id = await _service.CriarAsync(dto);
                return Ok(ApiResponse<int>.Ok(id));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail("Erro inesperado ao criar o cliente."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClienteDto dto)
        {
            if (id != dto.ClienteId)
                return BadRequest(ApiResponse<string>.Fail("ID da URL não confere com o corpo"));

            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(ApiResponse<string>.Fail(validation.Errors.Select(e => e.ErrorMessage)));
            try
            {
                await _service.AtualizarAsync(dto);
                return Ok(ApiResponse.Ok());
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail("Erro inesperado ao criar o cliente."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.ExcluirAsync(id);
            return Ok(ApiResponse.Ok());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.ObterPorIdAsync(id);
            return result == null
                ? NotFound(ApiResponse<string>.Fail("Cliente não encontrado."))
                : Ok(ApiResponse<ClienteListarDto>.Ok(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? nome, [FromQuery] int? categoriaId)
        {
            var result = await _service.ListarAsync(nome, categoriaId);
            return Ok(ApiResponse<IEnumerable<ClienteListarDto>>.Ok(result));
        }
    }
}
