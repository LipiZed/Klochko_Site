using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Klochko_Site.Controllers;
using Klochko_Site.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Klochko_Site.Tests
{
    public class ClientControllerTests
    {
        private readonly Mock<GeneralPostDbContext> _contextMock;
        private readonly ClientController _controller;

        public ClientControllerTests()
        {
            _contextMock = new Mock<GeneralPostDbContext>();
            _controller = new ClientController(_contextMock.Object);
        }

        // 1. Тест для метода Index: если клиент не найден, возвращается NotFound
        [Fact]
        public async Task Index_ReturnsNotFound_WhenCustomerNotFound()
        {
            // Подготавливаем фейковый набор данных, где нет клиента с указанным id
            _contextMock.Setup(c => c.Customers).ReturnsDbSet(new List<Customer>());

            // Act
            var result = _controller.Index(999); // Пытаемся найти несуществующего клиента

            // Assert
            Assert.IsType<NotFoundResult>(result); // Ожидаем NotFound, так как клиент не найден
        }

    }
    public class HomeControllerTests
    {
        private readonly Mock<GeneralPostDbContext> _contextMock;
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            // Создание mock-объектов для контекста и логера
            _contextMock = new Mock<GeneralPostDbContext>();
            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object, _contextMock.Object);
        }

        [Fact]
        public async Task Register_Post_ReturnsRedirectToLogin_WhenModelIsValid()
        {
            // Arrange: Создание модели регистрации с валидными данными
            var model = new UserRegisterViewModel
            {
                FullName = "Test User",
                Phone = "123-456-7890",
                Email = "testuser@example.com",
                Address = "123 Test St",
                Username = "testuser",
                Password = "SecurePassword123"
            };

            // Настройка mock-объекта для добавления клиента и учетных данных
            var customerList = new List<Customer>();
            var userCredentialList = new List<UserCredential>();
            _contextMock.Setup(c => c.Customers).ReturnsDbSet(customerList);
            _contextMock.Setup(c => c.UserCredentials).ReturnsDbSet(userCredentialList);

            // Act: Вызов метода Register с валидной моделью
            var result = await _controller.Register(model);

            // Assert: Проверяем, что метод вернул RedirectToAction (перенаправление на страницу входа)
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName); // Ожидаем редирект на Login
            Assert.Equal("Account", redirectResult.ControllerName); // Ожидаем редирект на контроллер Account

            // Проверяем, что в контексте добавлен новый клиент и учетная запись
            _contextMock.Verify(c => c.Customers.Add(It.Is<Customer>(customer => customer.FullName == model.FullName)), Times.Once);
            _contextMock.Verify(c => c.UserCredentials.Add(It.Is<UserCredential>(user => user.Username == model.Username)), Times.Once);
        }
    }
   
}
