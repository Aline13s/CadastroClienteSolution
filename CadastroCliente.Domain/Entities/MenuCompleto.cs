using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Domain.Entities
{
    public class MenuCompleto
    {
        public int MenuId { get; set; }
        public string MenuChave { get; set; }
        public string MenuTitulo { get; set; }

        public int PaginaId { get; set; }
        public string PaginaChave { get; set; }
        public string PaginaTitulo { get; set; }
        public string Rota { get; set; }

        public int? ControleId { get; set; }
        public string? ControleChave { get; set; }
        public string? ControleLabel { get; set; }

        public int? FiltroId { get; set; }
        public string? FiltroCampo { get; set; }
        public string? FiltroLabel { get; set; }
    }
}
