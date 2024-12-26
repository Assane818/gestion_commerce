using WebGesCommande.Enum;

namespace WebGesCommande.Models;

public class Payement : AbstractEntity
{
    public TypePayement TypePayement { get; set; }
    public Commande Commande { get; set; }
    public int CommandeId { get; set; }
    public bool  Remise { get; set; } = false;
    public string Reference { get; set; }
}