using Klochko_Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Klochko_Site.Controllers
{
    [Authorize(Roles = "admin")]
    public class PagesController : Controller
    {
        private readonly GeneralPostDbContext _context;

        public PagesController(GeneralPostDbContext context)
        {
            _context = context;
        }

        // 1. Управление пользователями: Просмотр списка пользователей
        public async Task<IActionResult> UserList()
        {
            // Получение данных из базы данных
            var users = await _context.UserCredentials.ToListAsync();
            var employees = await _context.Employees.ToListAsync();
            var branches = await _context.Branches.ToListAsync();

            // Создание кортежа из списка пользователей и списка сотрудников
            var model = new Tuple<IEnumerable<UserCredential>, IEnumerable<Employee>, IEnumerable<Branch>>(users, employees, branches);

            // Передача модели в представление
            return View(model);
        }

        // 2. Управление пользователями: Создание нового пользователя
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCredential user)
        {
            if (ModelState.IsValid)
            {
                // Добавляем нового пользователя в базу данных
                _context.UserCredentials.Add(user);
                await _context.SaveChangesAsync();

                // Перенаправляем обратно на страницу со списком пользователей
                return RedirectToAction(nameof(UserList));
            }

            // В случае ошибки возвращаемся на ту же страницу
            return View("UserList", await _context.UserCredentials.ToListAsync());
        }

        // 3. Управление пользователями: Редактирование пользователя
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.UserCredentials.FindAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserCredential user)
        {
            if (ModelState.IsValid)
            {
                // Обновляем данные пользователя в базе данных
                _context.UserCredentials.Update(user);
                await _context.SaveChangesAsync();

                // Перенаправляем обратно на страницу со списком пользователей
                return RedirectToAction(nameof(UserList));
            }

            // В случае ошибки возвращаемся на ту же страницу
            return View("UserList", await _context.UserCredentials.ToListAsync());
        }


        // 4. Управление пользователями: Удаление пользователя
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.UserCredentials.FindAsync(id);
            if (user != null)
            {
                _context.UserCredentials.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(UserList));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserList));
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserList));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(UserList));
        }

        [HttpPost]
        public async Task<IActionResult> AddBranch(Branch branch)
        {
            if (ModelState.IsValid)
            {
                // Добавление нового отделения в базу данных
                _context.Branches.Add(branch);
                await _context.SaveChangesAsync();

                // Перенаправление на страницу с обновленным списком отделений
                return RedirectToAction(nameof(UserList));
            }

            // Если модель не прошла проверку, верните текущий список данных
            return View("UserList", new Tuple<IEnumerable<UserCredential>, IEnumerable<Employee>, IEnumerable<Branch>>(
                await _context.UserCredentials.ToListAsync(),
                await _context.Employees.ToListAsync(),
                await _context.Branches.ToListAsync()
            ));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(UserList));
        }

        [HttpPost]
        public async Task<IActionResult> EditBranch(Branch branch)
        {
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserList));
        }

    }
}
