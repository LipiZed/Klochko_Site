using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Klochko_Site.Models;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly UserCredentialService _userCredentialService;

    public AccountController(UserCredentialService userCredentialService)
    {
        _userCredentialService = userCredentialService;
    }

    // Страница входа
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // Обработка входа
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _userCredentialService.ValidateCredentials(username, password);

        if (user != null)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            // Перенаправление к первоначально запрошенному URL
            string returnUrl = Request.Query["ReturnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
        return View();
    }

    // Выход пользователя
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home"); // Перенаправление на главную страницу после выхода
    }

    // Страница доступа запрещена
    public IActionResult AccessDenied()
    {
        return View();
    }
}
