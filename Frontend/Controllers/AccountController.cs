using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class AccountController : Controller
{
    private readonly AuthApiService _authApiService;

    public AccountController(AuthApiService authApiService)
    {
        _authApiService = authApiService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authApiService.LoginAsync(
            model.Email,
            model.Password
        );

        if (result == null)
        {
            ModelState.AddModelError(
                string.Empty,
                "Email ou mot de passe incorrect."
            );

            return View(model);
        }

        HttpContext.Session.SetString(
            "JwtToken",
            result.Token
        );

        HttpContext.Session.SetString(
            "UserEmail",
            result.Email
        );

        return Redirect("/Patients");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return Redirect("/Account/Login");
    }
}