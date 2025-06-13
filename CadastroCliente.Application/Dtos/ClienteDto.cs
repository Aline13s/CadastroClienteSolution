using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Application.Dtos
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public int? CategoriaId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        // Somente leitura (opcional para listagem)
        public string? Categoria { get; set; } // não usada no mapeamento de entrada
    }

}
