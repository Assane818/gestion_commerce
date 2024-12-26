using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebGesCommande.Enum;
using WebGesCommande.Models;
using WebGesCommande.Services;

namespace WebGesCommande.Controllers;

[Authorize(Roles = "RS")]
public class LivraisonController : Controller
{
    private readonly ILivraisonService _livraisonService;
    private readonly ILivreurService _livreurService;
    private readonly ICommandeService _commandeService;

    public LivraisonController(ILivraisonService livraisonService, ILivreurService livreurService, ICommandeService commandeService)
    {
        _livraisonService = livraisonService;
        _livreurService = livreurService;
        _commandeService = commandeService;
    }


    [HttpPost]
    [Authorize(Roles = "RS")]
    public async Task<IActionResult> Create(int livreurId, DateTime dateLivraison, string adresseLivraison, int CommandeId)
    {
        var livraison = new Livraison();
        var livreur = await _livreurService.SelectById(livreurId)!;
        var commande = await _commandeService.SelectById(CommandeId)!;
        livraison.LivreurId = livreurId;
        livraison.DateLivraison = DateTime.SpecifyKind(dateLivraison, DateTimeKind.Utc);
        livraison.AddressLivraison = adresseLivraison;
        livraison.CommandeId = CommandeId;
        livreur!.IsDisponible = false;
        commande!.EtatLivraison = EtatLivraison.LIVRER;
        await _commandeService.Update(commande);
        await _livreurService.Update(livreur);
        await _livraisonService.Insert(livraison);       
        return RedirectToAction("Index", "Commande");
    }
    
}