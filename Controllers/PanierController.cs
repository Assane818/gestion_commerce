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

namespace WebGesCommande.Controllers;
[Authorize(Roles = "CLIENT")]
public class PanierController : Controller
{
    private readonly IProduitService _produitService;
    private readonly ISession<Panier> _session;

    public PanierController(IProduitService produitService, ISession<Panier> session)
    {
        _produitService = produitService;
        _session = session;
    }

    [Authorize(Roles = "CLIENT")]
    public async Task<IActionResult> Index()
    {
       var panier = _session.Get("panier");
        return View(panier);
    }

    [HttpPost]
    [Authorize(Roles = "CLIENT")]
    public async Task<IActionResult> AddDetail([FromBody] JsonElement data)
    {
        var detail = new Detail();
        var produitId = data.GetProperty("Id").GetInt32();
        var produit = await _produitService.SelectById(produitId)!;
        detail.Produit = produit;
        detail.ProduitId = produitId;
        detail.QteCommande = 1;
        var panier = _session.Get("panier") ?? new Panier();
        panier.SetDetails(detail);
        _session.Set("panier", panier);
        Console.WriteLine(panier.Details.Count);
        return Ok(new { count = panier.Details.Count });
    }

    [HttpPost]
    [Authorize(Roles = "CLIENT")]
    public async Task<IActionResult> DecrementQteCommande([FromBody] JsonElement data)
    {
        var produitId = data.GetProperty("Id").GetInt32();
        var panier = _session.Get("panier")!;
        foreach (var detail in panier.Details)
        {
            if (detail.Produit.Id == produitId)
            {
                panier.DecrementQte(detail);
            }
        }
        _session.Set("panier", panier);
        return Ok(new { count = panier.Details.Count });
    }

    [HttpPost]
    [Authorize(Roles = "CLIENT")]
    public async Task<IActionResult> IncrementQteCommande([FromBody] JsonElement data)
    {
        var produitId = data.GetProperty("Id").GetInt32();
        var panier = _session.Get("panier")!;
        foreach (var detail in panier.Details)
        {
            if (detail.Produit.Id == produitId)
            {
                panier.IncrementQte(detail);
            }
        }
        _session.Set("panier", panier);
        return Ok(new { count = panier.Details.Count });
    }

    [HttpPost]
    [Authorize(Roles = "CLIENT")]
    public async Task<IActionResult> RemoveDetail([FromBody] JsonElement data)
    {
        var produitId = data.GetProperty("Id").GetInt32();
        var panier = _session.Get("panier")!;
        foreach (var detail in panier.Details)
        {
            if (detail.Produit.Id == produitId)
            {
                panier.RemoveDetail(detail);
                break;
            }
        }
        _session.Set("panier", panier);
        return Ok(new { count = panier.Details.Count });
    }

    [HttpPost]
    [Authorize(Roles = "CLIENT")]
    public async Task<IActionResult> ClearPanier()
    {
        _session.Remove("panier");
        return Ok();
    }
}
