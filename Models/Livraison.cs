namespace WebGesCommande.Models;

public class Livraison : AbstractEntity
{
    public Livreur Livreur { get; set; }
    public int LivreurId { get; set; }
    public Commande Commande { get; set; }
    public int CommandeId { get; set; }
    public string AddressLivraison { get; set; }
    public DateTime DateLivraison { get; set; }
}