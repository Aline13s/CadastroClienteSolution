using CadastroCliente.Application.Dtos;
using CadastroCliente.Application.Interfaces;
using CadastroCliente.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CadastroCliente.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriasController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _service.ListarAsync();
            return Ok(ApiResponse<IEnumerable<CategoriaDto>>.Ok(categorias));
        }
    }
}
