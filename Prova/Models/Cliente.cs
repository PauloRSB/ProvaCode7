using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prova.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public decimal Credito { get; set; }
        public string Telefone { get; set; }
        public int IdStatus { get; set; }
    }
    public class CliStatus
    {
        public Cliente Cliente { get; set; }
        public Status Status { get; set; }
    }
}
