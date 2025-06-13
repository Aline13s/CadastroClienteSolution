using CadastroCliente.Application.Dtos;
using CadastroCliente.Application.Interfaces;
using CadastroCliente.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CadastroCliente.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _service;

        public MenuController(IMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.ObterMenuCompletoAsync();
            return Ok(ApiResponse<IEnumerable<MenuDto>>.Ok(result));
        }
    }
}
