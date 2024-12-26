using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebGesCommande.Enum;
using WebGesCommande.Models;
using WebGesCommande.Services;

namespace WebGesCommande.Controllers;

[Authorize(Roles = "CLIENT")]
public class PayementController : Controller
{
    private readonly ICommandeService _commandeService;
    private readonly IPayementService _payementService;
    private readonly IClientService _clientService;

    public PayementController(ICommandeService commandeService, IPayementService payementService, IClientService clientService)
    {
        _commandeService = commandeService;
        _payementService = payementService;
        _clientService = clientService;
    }


    [HttpPost]
    [Authorize(Roles = "CLIENT")]
    public async Task<IActionResult> Create(int CommandeId, TypePayement modePaiement, string reference)
    {
        var payement = new Payement();
        var client = await _clientService.SelectByLogin(User.Identity.Name)!;
        int totalCommandesInMonth = (await _commandeService.SelectByClientAndMonth(client.Id)).Count();
        if (totalCommandesInMonth >= 10) {
            payement.Remise = true;
        }
        var commande = await _commandeService.SelectById(CommandeId)!;
        payement.CommandeId = CommandeId;
        payement.TypePayement = modePaiement;
        payement.Reference = reference;
        commande.IsPaye = true;
        await _payementService.Insert(payement);
        await _commandeService.Update(commande);
        return RedirectToAction("Index", "Commande");
    }
    
}