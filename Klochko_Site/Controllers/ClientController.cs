using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Klochko_Site.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Klochko_Site.Controllers
{
    [Authorize(Roles = "client")]
    public class ClientController : Controller
    {
        private readonly GeneralPostDbContext _context;

        public ClientController(GeneralPostDbContext context)
        {
            _context = context;
        }

        // Главная страница клиента
        public IActionResult Index(int customerId)
        {
            var customer = _context.Customers
                .Include(c => c.PackageSenders)
                    .ThenInclude(p => p.Receiver)    // Загрузка получателя
                .Include(c => c.PackageSenders)
                    .ThenInclude(p => p.Type)        // Загрузка типа посылки
                .Include(c => c.PackageReceivers)
                    .ThenInclude(p => p.Sender)      // Загрузка данных отправителя для полученных посылок
                .Include(c => c.PackageReceivers)
                    .ThenInclude(p => p.Type)        // Загрузка типа для полученных посылок
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // Метод подтверждения получения посылки
        [HttpPost]
        public async Task<IActionResult> ConfirmDelivery(int packageId)
        {
            var package = await _context.Packages
                    .FirstOrDefaultAsync(p => p.PackageId == packageId);


            if (package == null)
            {
                return NotFound("Посылка не найдена.");
            }

            // Проверка статуса посылки перед обновлением
            if (package.Status == "В пути")
            {
                package.Status = "Получена";
                package.DateDelivered = DateTime.Now;

                // Обновление таблицы DeliveryStatus
                var deliveryStatus = new DeliveryStatus
                {
                    PackageId = packageId,
                    DateUpdated = DateTime.Now,
                    Location = "Местонахождение клиента",
                    StatusDescription = "Получена"
                };

                _context.DeliveryStatuses.Add(deliveryStatus);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Посылка успешно подтверждена как полученная!";
            }
            else
            {
                TempData["Message"] = "Посылка уже подтверждена как доставленная.";
            }

            return RedirectToAction("Index", new { customerId = package.ReceiverId });
        }

        public IActionResult SendPackage()
        {
            ViewData["Branches"] = _context.Branches.ToList();
            ViewData["PackageTypes"] = _context.PackageTypes.ToList();
            ViewData["Customers"] = _context.Customers.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendPackage(int receiverId, int branchId, int typeId, decimal weight, string dimensions)
        {
            var userCredential = await _context.UserCredentials
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

            var senderId = userCredential.CustomerId;
            var newPackage = new Package
            {
                SenderId = (int)senderId,
                ReceiverId = receiverId,
                BranchId = branchId,
                TypeId = typeId,
                Weight = weight,
                Dimensions = dimensions,
                Status = "В пути", // Устанавливаем статус по умолчанию
                DateSent = DateTime.Now
            };

            _context.Packages.Add(newPackage);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Посылка успешно отправлена!";
            return RedirectToAction("Index", new { customerId = senderId });
        }

        public async Task<IActionResult> Dashboard()
        {
            var userCredential = await _context.UserCredentials
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

            if (userCredential?.CustomerId != null)
            {
                ViewBag.CustomerId = userCredential.CustomerId;
            }

            return View();
        }
    }
}
