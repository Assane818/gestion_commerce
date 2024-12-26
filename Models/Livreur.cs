namespace WebGesCommande.Models;

public class Livreur : Personne
{
    public bool IsDisponible { get; set; } = true;
    public ICollection<Livraison> livraisons { get; set; }
}