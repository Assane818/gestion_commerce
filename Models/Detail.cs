namespace WebGesCommande.Models;

public class Detail : AbstractEntity
{
    public double QteCommande { get; set; }
    public Produit Produit { get; set; }
    public int ProduitId { get; set; }
    public Commande Commande { get; set; }
    public int CommandeId { get; set; }
}