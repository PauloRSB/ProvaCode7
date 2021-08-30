using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prova.Models;

namespace Prova.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultController : ControllerBase
    {
        private readonly LojaContexto _context;

        public ConsultController(LojaContexto context)
        {
            _context = context;
        }

        //api/Consult/Token
        //Bearer Token de 2 horas
        [HttpGet("Token")]
        [AllowAnonymous]
        public string GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("fedaf7d8863b48e197b9287d492b708e");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //api/Consult/Produto
        [HttpPost("Produto")]
        [Authorize]
        public ReturnConsult CreateProduto(JObject objeto)
        {
            ReturnConsult retorno = new ReturnConsult();
            try
            {
                Produto produto = JsonConvert.DeserializeObject<Produto>(objeto.ToString());

                if (ProdutoExists(produto.CodProduto))
                {
                    retorno.Vendas = null;
                    retorno.Mensagem = "Erro: Produto já existe";
                    return retorno;
                }

                _context.Add(produto);
                _context.SaveChangesAsync();

                retorno.Vendas = null;
                retorno.Mensagem = "Produto Adicionado";
                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Vendas = null;
                retorno.Mensagem = "Erro: " + ex.Message;

                return retorno;
            }
        }

        // GET: api/Consult/Vendas
        [HttpGet("Vendas")]
        [Authorize]
        public ReturnConsult GetOferta(JObject objeto)
        {
            ReturnConsult retorno = new ReturnConsult();
            try
            {
                //instancia de objetos
                ReceiveConsult consult = JsonConvert.DeserializeObject<ReceiveConsult>(objeto.ToString());
                Cliente cliente = new Cliente();
                Produto produto = new Produto();
                List<Oferta> ofertas = new List<Oferta>();

                //captura de infos
                if (!string.IsNullOrEmpty(consult.Cpf))
                    cliente = _context.Clientes.FirstOrDefault(c => c.Cpf == consult.Cpf);

                if (!string.IsNullOrEmpty(consult.Produto))
                    produto = _context.Produto.FirstOrDefault(p => p.CodProduto == consult.Produto);


                //Filtro
                if (!string.IsNullOrEmpty(consult.Produto) && !string.IsNullOrEmpty(consult.Cpf))
                {
                    ofertas = _context.Oferta.Where(o => o.IdProduto == produto.CodProduto && o.IdCliente == cliente.Id).ToList();
                }
                if (string.IsNullOrEmpty(consult.Produto) && !string.IsNullOrEmpty(consult.Cpf))
                {
                    ofertas = _context.Oferta.Where(o => o.IdCliente == cliente.Id).ToList();
                }
                if (!string.IsNullOrEmpty(consult.Produto) && string.IsNullOrEmpty(consult.Cpf))
                {
                    ofertas = _context.Oferta.Where(o => o.IdProduto == produto.CodProduto).ToList();
                }
                if (string.IsNullOrEmpty(consult.Produto) && string.IsNullOrEmpty(consult.Cpf))
                {
                    ofertas = _context.Oferta.ToList();
                }
                retorno.Vendas = SetInfos(ofertas);
                retorno.Mensagem = "OK";

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Vendas = null;
                retorno.Mensagem = "Erro: " + ex.Message;

                return retorno;
            }

        }
        private List<Consult> SetInfos(List<Oferta> ofertas)
        {
            // cria objeto que sera retornado
            List<Consult> ListaVendas = new List<Consult>();
            Cliente cliente = new Cliente();
            Produto produto = new Produto();

            foreach (var item in ofertas)
            {
                Consult consulta = new Consult();

                cliente = _context.Clientes.FirstOrDefault(c => c.Id == item.IdCliente);
                produto = _context.Produto.FirstOrDefault(p => p.CodProduto == item.IdProduto);

                consulta.Cliente = cliente.Nome;
                consulta.Cpf = cliente.Cpf;
                consulta.Produto = produto.Descricao;
                consulta.Valor = produto.Preco;
                consulta.CodigoOferta = item.Id.ToString();

                ListaVendas.Add(consulta);
            }
            return ListaVendas;
        }
        private bool ProdutoExists(string id)
        {
            return _context.Produto.Any(e => e.CodProduto == id);
        }

    }
}
