
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGesCommande.Data;
using WebGesCommande.Models;
using WebGesCommande.Services;

namespace WebGesCommande.Controllers
{
    [Authorize(Roles = "CLIENT, RS")]
    public class ProduitController : Controller
    {
        private readonly IProduitService _produitService;
        private readonly IWebHostEnvironment _environment;

        public ProduitController(IProduitService produitService, IWebHostEnvironment environment)
        {
            _produitService = produitService;
            _environment = environment;
        }

        // GET: Roduit
        public async Task<IActionResult> Index(int pageNumber = 1, int limit = 9)
        {
            var produits = await _produitService.SelectArticlePaginate(pageNumber, limit);
            int totalProduits = (await _produitService.SelectAll()).Count();
            int maxPages = (int)Math.Ceiling((double)totalProduits / limit);
            ViewBag.MaxPages = maxPages;
            ViewBag.CurrentPage = pageNumber;
            return View(produits);
        }

        // GET: Roduit/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var produit = await _context.Produits
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (produit == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(produit);
        // }

        // GET: Roduit/Create
        [Authorize(Roles = "RS")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RS")]
        public async Task<IActionResult> Create(Produit produit)
        {
            ModelState.Remove("Details");
            ModelState.Remove("Image");
            if (ModelState.IsValid && (produit.QuantiteSeuil > produit.Quantite))
            {
                if (produit.ImageFile != null && produit.ImageFile.Length > 0)
                {
                    // Chemin du répertoire pour enregistrer l'image
                    var filePath = Path.Combine(_environment.WebRootPath, "img", produit.ImageFile.FileName);

                    // Créer le répertoire si nécessaire
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    // Enregistrer l'image sur le serveur
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await produit.ImageFile.CopyToAsync(stream);
                    }

                    produit.Image = produit.ImageFile.FileName;
                }
                await _produitService.Insert(produit);
                return RedirectToAction("Index");
            }
            return View(produit);
        }

        [Authorize(Roles = "RS")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _produitService.SelectById(id);
            if (produit == null)
            {
                return NotFound();
            }
            return View(produit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RS")]
        public async Task<IActionResult> Edit(int id, Produit produit, string image)
        {
            Console.WriteLine("❌❌❌❌❌❌❌❌❌❌");
            Console.WriteLine(image);
            if (ModelState.IsValid)
            {
                if (produit.ImageFile != null && produit.ImageFile.Length > 0)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, "img", produit.ImageFile.FileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await produit.ImageFile.CopyToAsync(stream);
                    }
                    produit.Image = produit.ImageFile.FileName;
                    Console.WriteLine("🔥🔥🔥🔥🔥🔥🔥🔥🔥");
                    Console.WriteLine(produit.Image);
                } else {
                    produit.Image = image;
                }
                await _produitService.Update(produit);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "RS")]
        public async Task<IActionResult> Delete(int id)
        {
            var produit = await _produitService.SelectById(id)!;
            await _produitService.Delete(produit);
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitExists(int id)
        {
            return _produitService.SelectById(id) != null;
        }
    }
}
