using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website.Domains.Access.ViewModels;

namespace Website.Domains.Access.Controllers;

public class AccessController : Controller
{
     public IActionResult Login()
     {
        ClaimsPrincipal claimUser = HttpContext.User;

        if (claimUser.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        return View();
     }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel modelLogin)
    {
        if (modelLogin.Email == "user@example.com" &&
            modelLogin.Password == "123")
        {
            List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
                    new Claim("OtherProperties", "Example Role")
                };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = modelLogin.KeepLoggedIn
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Home");
        }

        ViewData["ValidateMEssage"] = "user not found";
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }


    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login", "Access");
    }
}

