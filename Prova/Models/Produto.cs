using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prova.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Preco { get; set; }
        public string Tipo { get; set; }
        public string CodProduto { get; set; }
    }
}
