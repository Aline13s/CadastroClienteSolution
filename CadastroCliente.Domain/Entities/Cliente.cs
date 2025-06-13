using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Domain.Entities
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public int CategoriaId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public Categoria Categoria { get; set; }  // Navegação
    }
}
