using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prova.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string FinalizaCliente { get; set; }
        public string ContabilizaVenda { get; set; }
        public int CodStatus { get; set; }
    }
}
