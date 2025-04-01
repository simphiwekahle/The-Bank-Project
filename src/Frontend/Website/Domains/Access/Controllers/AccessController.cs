using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website.Domains.Access.ViewModels;
using Website.Domains.Persons.Services;

namespace Website.Domains.Access.Controllers;

public class AccessController(
    IPersonsServices personsServices) : Controller
{
     public IActionResult Login()
     {
        ClaimsPrincipal? claimUser = HttpContext.User;

        if (claimUser.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        return View();
     }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel modelLogin)
    {
        if (ModelState.IsValid)
        {
            var findPerson = (await personsServices.GetPersonsAsync())?
            .FindAll(p =>
                p.Email == modelLogin.Email &&
                p.Password == modelLogin.Password);

            if (findPerson is not null)
            {
                List<Claim> claims = [
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
                    new Claim("OtherProperties", "Example Role")
                ];
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
        }
        ViewData["ValidateMessage"] = "user not found";
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

