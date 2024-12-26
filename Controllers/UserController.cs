using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebGesCommande.Enum;
using WebGesCommande.Models;
using WebGesCommande.Services;
using WebService.Services;

namespace WebGesCommande.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IClientService _clientService;

        public UserController(IUserService userService, IClientService clientService)
        {
            _userService = userService;
            _clientService = clientService;
        }

        public async Task<IActionResult> Connexion()
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            Console.WriteLine(HttpContext.User);
            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Connexion", "User");
        }
        [HttpPost]
        public async Task<IActionResult> Connexion(User user)
        {
            User? userconnect = await _userService.Authenticate(user.login, user.Password);
            if (userconnect != null)
            {
                // Créez les claims pour l'utilisateur
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, userconnect.login),
                    new Claim(ClaimTypes.Role, userconnect.Role.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userconnect.Id.ToString())

                };
                // Créez l'identité et les propriétés d'authentification
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                };

                // Connectez l'utilisateur
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Login ou mot de passe incorrect";
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user, string address)
        {
            ModelState.Remove("Role");
            if (ModelState.IsValid)
            {
                var client = new Client { Address = address };
                user.Role = Role.CLIENT;
                client.User = user;
                await _clientService.Insert(client);
                return RedirectToAction("Connexion");
            }
            return View(user);
        }
    }
}
