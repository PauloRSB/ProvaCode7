using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prova.Models;

namespace Prova.Controllers
{
    public class OfertasController : Controller
    {
        private readonly LojaContexto _context;

        public OfertasController(LojaContexto context)
        {
            _context = context;
        }

        // GET: Ofertas
        public async Task<IActionResult> Index(string filtro)
        {
            ViewData["filtro"] = filtro;

            List<Cliente> clientes = await _context.Clientes.Where(c => c.IdStatus == 1 || c.IdStatus == 15 || c.IdStatus == 19).ToListAsync();

            if (!String.IsNullOrEmpty(filtro))
                clientes = clientes.Where(c => c.Cpf.Contains(filtro) || c.Nome.Contains(filtro)).ToList();

            List<CliStatus> lista = new List<CliStatus>();


            foreach (var item in clientes)
            {
                CliStatus cliStatus = new CliStatus();
                cliStatus.Cliente = item;
                cliStatus.Status = await _context.Status.FirstOrDefaultAsync(s => s.CodStatus == item.IdStatus);

                lista.Add(cliStatus);
            }

            return View(lista);
        }

        // GET: Ofertas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // GET: Ofertas/Create
        public IActionResult Create(int idCliente)
        {
            OfertaView ofertaView = new OfertaView();
            ofertaView.Cliente = _context.Clientes.FirstOrDefault(c => c.Id == idCliente);
            ofertaView.Status = _context.Status.FirstOrDefault(s => s.CodStatus == ofertaView.Cliente.IdStatus);

            ViewData["StatusList"] = new SelectList(_context.Status.ToList(), "CodStatus", "Descricao");
            ViewData["ProdutosList"] = new SelectList(_context.Produto.ToList(), "CodProduto", "Descricao");

            return View(ofertaView);
        }

        // POST: Ofertas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OfertaView ofertaView)
        {
            Oferta oferta = new Oferta();

            Produto produto = _context.Produto.FirstOrDefault(p => p.CodProduto == ofertaView.Produto.CodProduto);

            Cliente cliente = _context.Clientes.FirstOrDefault(c => c.Id == ofertaView.Cliente.Id);
            decimal preco = Convert.ToDecimal(produto.Preco.Replace(".",","));
            cliente.Credito = cliente.Credito - preco;
            cliente.Nome = ofertaView.Cliente.Nome;
            cliente.Telefone = ofertaView.Cliente.Telefone;
            cliente.IdStatus = ofertaView.Status.CodStatus;
            _context.Update(cliente);
            await _context.SaveChangesAsync();

            if(produto.Tipo == "HARDWARE")
            {
                _context.Add(ofertaView.Endereco);
                await _context.SaveChangesAsync();
                EnderecoEntrega endereco = _context.Endereco.FirstOrDefault(e => e.Id == ofertaView.Endereco.Id);

                oferta.IdEndereco = endereco.Id;
            }

            oferta.IdCliente = cliente.Id;
            oferta.IdProduto = produto.CodProduto;
            _context.Add(oferta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Ofertas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferta.FindAsync(id);
            if (oferta == null)
            {
                return NotFound();
            }
            return View(oferta);
        }

        // POST: Ofertas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCliente,IdEndereco,IdProduto,IdStatus")] Oferta oferta)
        {
            if (id != oferta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oferta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfertaExists(oferta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(oferta);
        }

        // GET: Ofertas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // POST: Ofertas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oferta = await _context.Oferta.FindAsync(id);
            _context.Oferta.Remove(oferta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfertaExists(int id)
        {
            return _context.Oferta.Any(e => e.Id == id);
        }
    }
}
