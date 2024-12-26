using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebGesCommande.Core;
using WebGesCommande.Data;
using WebGesCommande.Enum;
using WebGesCommande.Models;
using WebGesCommande.Services;

namespace WebGesCommande.Controllers
{
    [Authorize(Roles = "CLIENT, RS")]
    public class CommandeController : Controller
    {
        private readonly ICommandeService _commandeService;
        private readonly IClientService _clientService;
        private readonly IProduitService _produitService;
        private readonly ILivreurService _livreurService;
        private readonly ISession<Panier> _session;

        public CommandeController(ICommandeService commandeService, ISession<Panier> session, IClientService clientService, IProduitService produitService, ILivreurService livreurService)
        {
            _commandeService = commandeService;
            _session = session;
            _clientService = clientService;
            _produitService = produitService;
            _livreurService = livreurService;
        }

        // GET: Commande
        [Authorize(Roles = "CLIENT, RS")]
        public async Task<IActionResult> Index(string? etat, string? paye, int pageNumber = 1, int limit = 2)
        {
            Client client = await _clientService.SelectByLogin(User.Identity.Name)!;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            IEnumerable<Commande> commandes;
            int totalPages = 0;
            int maxPages = 0;
            if (!string.IsNullOrEmpty(etat) && string.IsNullOrEmpty(paye)) {
                System.Enum.TryParse(etat, out EtatCommande etatCommande);
                commandes = await _commandeService.SelectByClientAndEtatPaginate(client.Id, etatCommande, pageNumber, limit);
                totalPages = (await _commandeService.SelectByClientAndEtat(client.Id, etatCommande)).Count();
                maxPages = (int)Math.Ceiling((double)totalPages / limit);
            }
            else if (!string.IsNullOrEmpty(paye) && string.IsNullOrEmpty(etat)) {
                bool isPaye = bool.Parse(paye);
                commandes = await _commandeService.SelectByClientAndPayementPaginate(client.Id, isPaye, pageNumber, limit);
                totalPages = (await _commandeService.SelectByClientAndPayement(client.Id, isPaye)).Count();
                maxPages = (int)Math.Ceiling((double)totalPages / limit);
            } 
            else if (!string.IsNullOrEmpty(etat) && !string.IsNullOrEmpty(paye)) {
                bool isPaye = bool.Parse(paye);
                System.Enum.TryParse(etat, out EtatCommande etatCommande);
                commandes = await _commandeService.SelectByClientAndFitreAllPaginate(client.Id, etatCommande, isPaye, pageNumber, limit);
                totalPages = (await _commandeService.SelectByClientAndFitreAll(client.Id, etatCommande, isPaye)).Count();
                maxPages = (int)Math.Ceiling((double)totalPages / limit);
            }
            else {
                if (role == "RS") {
                    commandes = await _commandeService.SelectByEtatPaginate(EtatCommande.VALIDER, pageNumber, limit);
                    totalPages = (await _commandeService.SelectByEtat(EtatCommande.VALIDER)).Count();
                    maxPages = (int)Math.Ceiling((double)totalPages / limit);
                } else {
                    commandes = await _commandeService.SelectByClientPaginate(client.Id, pageNumber, limit);
                    totalPages = (await _commandeService.SelectByClient(client.Id)).Count();
                    maxPages = (int)Math.Ceiling((double)totalPages / limit);
                }
            }
            ViewBag.MaxPages = maxPages;
            ViewBag.CurrentPage = pageNumber;
            return View(commandes);
        }
        

        // GET: Commande/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _commandeService.SelectById(id);
            if (commande == null)
            {
                return NotFound();
            }
            ViewBag.Livreur = new SelectList(await _livreurService.SelectAll(), "Id", "Prenom");
            return View(commande);
        }

        // GET: Commande/Create

        // POST: Commande/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var commande = new Commande();
            var panier = _session.Get("panier")!;
            var client = await _clientService.SelectByLogin(User.Identity.Name)!;
            int totalCommandesInMonth = (await _commandeService.SelectByClientAndMonth(client.Id)).Count();
            if (totalCommandesInMonth >= 10) {
                commande.Total = panier.Total * 0.1;
            } else {
                commande.Total = panier.Total;
            }
            commande.Client = client;
            if (await verifieArticleDispo(panier)) {
                commande.EtatCommande = EtatCommande.VALIDER;
            }
            foreach (var detail in panier.Details)
            {
                commande.Details.Add(detail);
                var produit = await _produitService.SelectById(detail.ProduitId);
                produit.Quantite -= detail.QteCommande;
                await _produitService.Update(produit);
                panier.RemoveProduitCommande(panier.Details);
            }
            await _commandeService.Insert(commande);
            _session.Remove("panier");
            return Ok();
        }

        public async Task<bool> verifieArticleDispo(Panier panier) {
            foreach (var detail in panier.Details)
            {
                var produit =  await _produitService.SelectById(detail.ProduitId);
                if (produit == null || produit.Quantite < detail.QteCommande)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<IActionResult> RemoveDetail(int id)
        {
            var panier = _session.Get("panier")!;
            foreach (var item in panier.Details)
            {
                if (item.ProduitId == id)
                {
                    panier.RemoveDetail(item);
                    break;
                }
            }
            _session.Set("panier", panier);
            return RedirectToAction("Create");
        }

        // GET: Commande/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var commande = await _context.Commandes.FindAsync(id);
        //     if (commande == null)
        //     {
        //         return NotFound();
        //     }
        //     ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", commande.ClientId);
        //     ViewData["LivreurId"] = new SelectList(_context.Livreurs, "Id", "Id", commande.LivreurId);
        //     return View(commande);
        // }

        // POST: Commande/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Total,ClientId,LivreurId,EtatCommande,Livraison,IsPaye,Id,CreateAt,UpdateAt")] Commande commande)
        // {
        //     if (id != commande.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(commande);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!CommandeExists(commande.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", commande.ClientId);
        //     ViewData["LivreurId"] = new SelectList(_context.Livreurs, "Id", "Id", commande.LivreurId);
        //     return View(commande);
        // }

        // GET: Commande/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var commande = await _context.Commandes
        //         .Include(c => c.Client)
        //         .Include(c => c.Livreur)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (commande == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(commande);
        // }

        // POST: Commande/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var commande = await _context.Commandes.FindAsync(id);
        //     if (commande != null)
        //     {
        //         _context.Commandes.Remove(commande);
        //     }

        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        // private bool CommandeExists(int id)
        // {
        //     return _context.Commandes.Any(e => e.Id == id);
        // }
    }
}
