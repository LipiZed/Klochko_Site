using Klochko_Site.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Klochko_Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeneralPostDbContext _context;
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, GeneralPostDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Метод для обработки регистрации
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Создание нового клиента
                var customer = new Customer
                {
                    FullName = model.FullName,
                    Phone = model.Phone,
                    Email = model.Email,
                    Address = model.Address,
                    DateRegistered = DateTime.Now
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                // Создание учетной записи
                var userCredential = new UserCredential
                {
                    Username = model.Username,
                    Password = model.Password, // В реальной ситуации используйте хеширование пароля
                    Role = "client",         // Назначение роли клиента
                    CustomerId = customer.CustomerId // Привязка к созданному клиенту
                };

                _context.UserCredentials.Add(userCredential);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account"); // Перенаправление на страницу входа
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
