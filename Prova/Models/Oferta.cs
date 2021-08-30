using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prova.Models
{
    public class Oferta
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdEndereco { get; set; }
        public string IdProduto { get; set; }
        
    }
    public class OfertaView
    {
        public Oferta Oferta { get; set; }
        public Cliente Cliente { get; set; }
        public EnderecoEntrega Endereco { get; set; }
        public Produto Produto { get; set; }
        public List<Produto> Produtos { get; set; }
        public Status Status { get; set; }
        public List<Status> NewStatus { get; set; }
    }
}
