using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prova.Models
{
    public class EnderecoEntrega
    {
        public int Id { get; set; }
        public int Cep { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
