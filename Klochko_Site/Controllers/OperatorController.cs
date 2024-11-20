using Klochko_Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klochko_Site.Controllers
{
    [Authorize(Roles = "operator")]
    public class OperatorController : Controller
    {
        private readonly GeneralPostDbContext _context;

        public OperatorController(GeneralPostDbContext context)
        {
            _context = context;
        }


        // 1. Просмотр посылок
        public IActionResult OperatorPage(string statusFilter, string searchQuery)
        {
            var packages = _context.Packages.AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter))
            {
                packages = packages.Where(p => p.Status == statusFilter);
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                packages = packages.Where(p => p.SenderId.ToString().Contains(searchQuery) || p.ReceiverId.ToString().Contains(searchQuery));
            }

            return View(packages.ToList());
        }

        // 2. Обновление статуса доставки
        [HttpPost]
        public async Task<IActionResult> UpdateDeliveryStatus(int packageId, string location, string statusDescription)
        {
            var newStatus = new DeliveryStatus
            {
                PackageId = packageId,
                Location = location,
                StatusDescription = statusDescription,
                DateUpdated = DateTime.Now
            };

            _context.DeliveryStatuses.Add(newStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // 3. Добавление транзакции
        [HttpPost]
        public async Task<IActionResult> AddTransaction(int packageId, string transactionType, decimal amount)
        {
            var transaction = new Transaction
            {
                PackageId = packageId,
                TransactionType = transactionType,
                Amount = amount,
                TransactionDate = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // 4. Добавление услуги
        [HttpPost]
        public async Task<IActionResult> AddService(string serviceName, string description, decimal price)
        {
            var service = new Service
            {
                ServiceName = serviceName,
                Description = description,
                Price = price
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // 5. Просмотр логов
        public IActionResult ViewLogs()
        {
            var logs = _context.Logs.OrderByDescending(log => log.LogDate).Take(100).ToList();
            return View("Logs", logs);
        }
    }
}
