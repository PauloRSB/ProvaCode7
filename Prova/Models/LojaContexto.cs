using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prova.Models;

namespace Prova.Models
{
    public class LojaContexto : DbContext
    {
        public LojaContexto(DbContextOptions<LojaContexto> opcoes) : base(opcoes)
        {

        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Oferta> Oferta { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<EnderecoEntrega> Endereco { get; set; }
    }
}
