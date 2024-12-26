namespace WebGesCommande.Models;

public class Panier
{
    public List<Detail> Details { get; } = new List<Detail>();
    public double Total { get; set; }
    public int ClientId { get; set; }
    public int nbreProduit { get; set; }

    public void SetDetails(Detail detail)
    {
        foreach (var item in Details)
        {
            if (item.ProduitId == detail.ProduitId)
            {
                item.QteCommande += detail.QteCommande;
                Total += detail.QteCommande * detail.Produit.Prix;
                return;
            }
        }
        Details.Add(detail);
        Total += detail.QteCommande * detail.Produit.Prix;

    }

    public void RemoveProduitCommande(List<Detail> details)
    {
        foreach (var detail in details)
        {
            detail.Produit = null;
        }
    }

    public void RemoveDetail(Detail detail)
    {
        Details.Remove(detail);
        Total -= detail.QteCommande * detail.Produit.Prix;
    }

    public void DecrementQte(Detail detail)
    {
        detail.QteCommande -= 1;
        Total -= detail.Produit.Prix;
    }

    public void IncrementQte(Detail detail)
    {
        detail.QteCommande += 1;
        Total += detail.Produit.Prix;
    }
}