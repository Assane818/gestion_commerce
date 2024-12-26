namespace WebGesCommande.Models;

public class Client : AbstractEntity
{
    public required string Address { get; set; }
    public double Solde { get; set; } = 0;
    public User User { get; set; }
    public int UserId { get; set; }
    public ICollection<Commande>? Commandes { get; set; }
}