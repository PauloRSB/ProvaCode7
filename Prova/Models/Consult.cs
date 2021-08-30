using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prova.Models
{
    public class ReceiveConsult
    {
        public string Cpf { get; set; }
        public string Produto { get; set; }
    }
    public class ReturnConsult
    {
        public List<Consult> Vendas { get; set; }
        public string Mensagem { get; set; }
    }
    public class Consult
    {
        public string Cliente { get; set; }
        public string Cpf { get; set; }
        public string Produto { get; set; }
        public string Valor { get; set; }
        public string CodigoOferta { get; set; }
    }

}
