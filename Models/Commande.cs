using System.ComponentModel.DataAnnotations.Schema;
using WebGesCommande.Enum;

namespace WebGesCommande.Models;

public class Commande : AbstractEntity
{
    public double Total { get; set; }
    public Client Client { get; set; }
    public int ClientId { get; set; }
    public EtatCommande EtatCommande { get; set; }= EtatCommande.ATENTE;
    public EtatLivraison EtatLivraison{ get; set; } = EtatLivraison.NONLIVRER;
    public bool IsPaye { get; set; } = false;
    public ICollection<Detail> Details { get; set; } = new List<Detail>();
    [NotMapped]
    public Payement? Payement { get; set; }
    [NotMapped]
    public Livraison? Livraison { get; set; }

}   