using Microsoft.AspNetCore.Mvc;

namespace WebGesCommande.Controllers;

public class SecurityController : Controller
{
    public IActionResult Error404()
    {
        return View();
    }
}