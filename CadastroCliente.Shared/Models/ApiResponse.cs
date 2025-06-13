using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Shared.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }     // ← propriedade
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        // ↓ método chamado Success() conflita com a propriedade acima
        public static ApiResponse<T> Ok(T data) => new() { Success = true, Data = data };
        public static ApiResponse<T> Fail(IEnumerable<string> errors) => new() { Success = false, Errors = errors };
        public static ApiResponse<T> Fail(string error) => new() { Success = false, Errors = new[] { error } };
    }

    public class ApiResponse
    {
        public static ApiResponse<string> Ok() => ApiResponse<string>.Ok("Operação realizada com sucesso.");
    }
}
