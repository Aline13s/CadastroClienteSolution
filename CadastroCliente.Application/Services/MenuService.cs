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
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repo;
        private readonly IMapper _mapper;

        public MenuService(IMenuRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuDto>> ObterMenuCompletoAsync()
        {
            var flat = await _repo.GetMenuCompletoAsync();
            var menuDict = new Dictionary<int, MenuDto>();

            foreach (var row in flat)
            {
                if (!menuDict.TryGetValue(row.MenuId, out var menuDto))
                {
                    menuDto = new MenuDto
                    {
                        MenuId = row.MenuId,
                        Chave = row.MenuChave,
                        Titulo = row.MenuTitulo,
                        Paginas = new List<PaginaDto>()
                    };
                    menuDict.Add(row.MenuId, menuDto);
                }

                var pagina = menuDto.Paginas.FirstOrDefault(p => p.PaginaId == row.PaginaId);
                if (pagina == null)
                {
                    pagina = new PaginaDto
                    {
                        PaginaId = row.PaginaId,
                        Chave = row.PaginaChave,
                        Titulo = row.PaginaTitulo,
                        Rota = row.Rota,
                        Controles = new List<ControleDto>(),
                        Filtros = new List<FiltroDto>()
                    };
                    menuDto.Paginas.Add(pagina);
                }

                if (row.ControleId.HasValue && !pagina.Controles.Any(c => c.ControleId == row.ControleId))
                {
                    pagina.Controles.Add(new ControleDto
                    {
                        ControleId = row.ControleId.Value,
                        PaginaId = row.PaginaId,
                        Chave = row.ControleChave!,
                        Label = row.ControleLabel!
                    });
                }

                if (row.FiltroId.HasValue && !pagina.Filtros.Any(f => f.FiltroId == row.FiltroId))
                {
                    pagina.Filtros.Add(new FiltroDto
                    {
                        FiltroId = row.FiltroId.Value,
                        PaginaId = row.PaginaId,
                        Campo = row.FiltroCampo!,
                        Label = row.FiltroLabel!
                    });
                }
            }

            return menuDict.Values;
        }
    }
}
