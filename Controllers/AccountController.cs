using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using AspStudio.Models;
using AspStudio.Data;
using System.Security.Claims;

namespace AspStudio.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext dbContext;

        public AccountController(ILogger<AccountController> logger, ApplicationDbContext _dbContext)
        {
            _logger = logger;
            dbContext = _dbContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginInfo)
        {
            if (dbContext.Usuarios.Count(p => p.Login.Equals(loginInfo.Login) && p.Password.Equals(loginInfo.Password)) > 0)
            {
                Usuario usuario = dbContext.Usuarios.FirstOrDefault(p => p.Login.Equals(loginInfo.Login) && p.Password.Equals(loginInfo.Password));
                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(usuario), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await this.HttpContext.SignInAsync("fhirlogin", principal);
                Console.WriteLine("Autenticado...");
                return RedirectToAction("Index", "Home");
            }
            else return View();
        }
        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync();
            Console.WriteLine("Logout...");
            return RedirectToAction("Login", "Account");
        }

            private IEnumerable<Claim> GetUserClaims(Usuario _usuario)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, _usuario.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, _usuario.Nombre));
            return claims;
        }

    }
}