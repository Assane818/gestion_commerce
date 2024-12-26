document.addEventListener("DOMContentLoaded", function () {
    const countPanier = document.getElementById("countPanier");

  window.addPanier = async function addPanier(id, libelle) {
    const data = {
        Id: id,
        Libelle: libelle,
      };
        await fetch("/Panier/AddDetail", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
        })
        .then(async (response) => {
            if (response.ok) {
                const result = await response.json();
                console.log("Panier ajouté");
                console.log("Nombre de détails dans le panier:", result.count);
                countPanier.innerText = result.count;
            } else {
                console.log("panier non ajouté");
            }
        })
    };

    window.decrementQte = async function (produitId) {
        const data = {
            Id: produitId,
        };
        await fetch("/Panier/DecrementQteCommande/", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        })
        .then(async (response) => {
            if (response.ok) {
                const result = await response.json();
                console.log("Panier décrémenté");
                countPanier.innerText = result.count;
                location.reload();
            } else {
                console.log("panier non ajouté");
            }
        });
    };

    window.incrementQte = async function (produitId) {
        const data = {
            Id: produitId,
        };
        await fetch("/Panier/IncrementQteCommande/", {
            method: "POST", 
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        })
        .then(async (response) => {
            if (response.ok) {
                const result = await response.json();
                console.log("Panier incrementé");
                countPanier.innerText = result.count;
                location.reload();
            } else {
                console.log("panier non ajouté");
            }
        });
    };

    window.remove = async function (produitId) {
        const data = {
            Id: produitId,
        };
        await fetch("/Panier/RemoveDetail/", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        })
        .then(async (response) => {
            if (response.ok) {
                const result = await response.json();
                console.log("detail supprimé");
                countPanier.innerText = result.count;
                location.reload();
            } else {
                console.log("panier non ajouté");
            }
        });
    };

    window.clearPanier = async function () {
        await fetch("/Panier/ClearPanier/", {
            method: "POST", 
        })
        .then(async (response) => {
            if (response.ok) {
                console.log("Panier vidé");
                location.reload();
            } else {
                console.log("panier non ajouté");
            }
        });
    };

    window.saveCommande = async function saveCommande() {
        await fetch("/Commande/Create/", {
            method: "POST", 
        })
        .then(async (response) => {
            if (response.ok) {
                console.log("Commande enregistrée");
                window.location.href = "/Commande/Index";
            } else {
                console.log("Commande non enregistrée");
            }
        });
    };
});
