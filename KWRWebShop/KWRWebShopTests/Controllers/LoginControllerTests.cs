using KWRWebShopAPI.Controllers;
using Microsoft.AspNetCore.Http;

namespace KWRWebShopTests.Controllers
{
    public class LoginControllerTests
    {
        private readonly LoginController _loginController;
        private readonly Mock<ILoginService> _loginServiceMock = new();

        public LoginControllerTests() 
        {
            _loginController = new(_loginServiceMock.Object);
        }

        [Fact]
        public async void GetAllLoginAsync_ShouldReturnStatus200_WhenLoginsExists()
        {

            // Arrange
            List<LoginResponse> logins = new()
            {
                new()
                {
                    LoginId = 1,
                    Email = "mail_1@test.dk",
                    Type = Role.Admin,
                },
                new()
                {
                    LoginId = 2,
                    Email = "mail_2@test.dk",
                    Type = Role.User,
                }
            };

            _loginServiceMock
                .Setup(x => x.GetAllLoginAsync())
                .ReturnsAsync(logins);

            //Act
            var result = (IStatusCodeActionResult) await _loginController.GetAllLoginAsync();

            //Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void GetAllLoginAsync_ShouldReturnStatus204_WhenNoLoginsExists()
        {
            // Arrange
            List<LoginResponse> logins = new();

            _loginServiceMock
                .Setup(x => x.GetAllLoginAsync())
                .ReturnsAsync(logins);

            // Act
            var result = (IStatusCodeActionResult)await _loginController.GetAllLoginAsync();

            // Assert
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async void GetAllLoginAsync_ShouldReturnStatus500_WhenExceptionIsRaised()
        {
            // Arrange
            List<LoginResponse> logins = new();

            _loginServiceMock
                .Setup(x => x.GetAllLoginAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _loginController.GetAllLoginAsync();


            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void FindLoginByIdAsync_ShouldReturnStatus200_WhenLoginsExists()
        {
            // Arrange
            int loginId = 1;

            LoginResponse login = new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = Role.Admin,
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", login }
                });

            // Set the mock HTTP context on the LoginController instance
            _loginController.ControllerContext.HttpContext = mockHttpContext.Object;

            _loginServiceMock
                .Setup(x => x.FindLoginByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(login);

            // Act
            var result = (IStatusCodeActionResult)await _loginController.FindLoginByIdAsync(loginId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }


        [Fact]
        public async void FindLoginByIdAsync_ShouldReturnStatus404_WhenLoginDoesNotExist()
        {
            // Arrange
            int currentUserId = 1;

            CustomerResponse customer = new()
            {
                CustomerId = currentUserId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _loginController.ControllerContext.HttpContext = mockHttpContext.Object;

            int loginId = 1;

            _loginServiceMock
                .Setup(x => x.FindLoginByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _loginController.FindLoginByIdAsync(loginId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void FindLoginByIdAsync_ShouldReturnStatus500_WhenExceptionIsRaised()
        {
            // Arrange
            int loginsId = 1;

            _loginServiceMock
                .Setup(x => x.FindLoginByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _loginController.FindLoginByIdAsync(loginsId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void CreateLoginAsync_ShouldReturnStatus200_WhenLoginIsSuccessfullyCreated()
        {
            // Arrange
            LoginRequest newLogin = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
            };

            int loginId = 1;

            LoginResponse loginResponse = new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = 0,
            };

            _loginServiceMock
                .Setup(x => x.CreateLoginAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(loginResponse);

            // Act
            var result = (IStatusCodeActionResult)await _loginController.CreateLoginAsync(newLogin);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void CreateLoginAsync_ShouldReturnStatus500_WhenExceptionIsRaised()
        {
            // Arrange
            LoginRequest newLogin = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
            };

            _loginServiceMock
                .Setup(x => x.CreateLoginAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _loginController.CreateLoginAsync(newLogin);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void UpdateLoginByIdAsync_ShouldReturnStatus200_WhenLoginExists()
        {
            // Arrange
            int currentUserId = 1;

            CustomerResponse customer = new()
            {
                CustomerId = currentUserId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _loginController.ControllerContext.HttpContext = mockHttpContext.Object;

            LoginRequest updatedLogin = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
            };

            int loginId = 1;

            LoginResponse loginResponse = new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = 0,
            };

            _loginServiceMock
                .Setup(x => x.UpdateLoginAsync(It.IsAny<int>(), It.IsAny<LoginRequest>()))
                .ReturnsAsync(loginResponse);

            // Act
            var result = (IStatusCodeActionResult) await _loginController.UpdateLoginByIdAsync(loginId, updatedLogin);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void UpdateLoginByIdAsync_ShouldReturnStatus404_WhenLoginDoesNotExist()
        {
            // Arrange
            int currentUserId = 1;

            CustomerResponse customer = new()
            {
                CustomerId = currentUserId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _loginController.ControllerContext.HttpContext = mockHttpContext.Object;

            LoginRequest updatedLogin = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
            };

            int loginId = 1;

            _loginServiceMock
                .Setup(x => x.UpdateLoginAsync(It.IsAny<int>(), It.IsAny<LoginRequest>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _loginController.UpdateLoginByIdAsync(loginId, updatedLogin);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void UpdateLoginByIdAsync_ShouldReturnStatus500_WhenExceptionIsRaised()
        {
            // Arrange
            LoginRequest updatedLogin = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
            };

            int loginId = 1;

            _loginServiceMock
                .Setup(x => x.UpdateLoginAsync(It.IsAny<int>(), It.IsAny<LoginRequest>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _loginController.UpdateLoginByIdAsync(loginId, updatedLogin);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void DeleteLoginByIdAsync_ShouldReturnStatus200_WhenLoginIsDeleted()
        {
            // Arrange
            int currentUserId = 1;

            CustomerResponse customer = new()
            {
                CustomerId = currentUserId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _loginController.ControllerContext.HttpContext = mockHttpContext.Object;

            int loginId = 1;

            LoginResponse loginResponse = new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = 0,
            };

            _loginServiceMock
                .Setup(x => x.DeleteLoginAsync(It.IsAny<int>()))
                .ReturnsAsync(loginResponse);

            // Act
            var result = (IStatusCodeActionResult) await _loginController.DeleteLoginByIdAsync(loginId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void DeleteLoginById_ShouldReturnStatus400_WhenLoginDoesNotExist()
        {
            // Arrange
            int currentUserId = 1;

            CustomerResponse customer = new()
            {
                CustomerId = currentUserId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _loginController.ControllerContext.HttpContext = mockHttpContext.Object;

            int loginId = 1;

            _loginServiceMock
                .Setup(x => x.DeleteLoginAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _loginController.DeleteLoginByIdAsync(loginId);

            // Assert
            Assert.Equal(404,result.StatusCode);
        }

        [Fact]
        public async void DeleteLoginById_ShouldReturnStatus500_WhenExceptionIsRaised()
        {
            // Arrange
            int loginId = 1;

            _loginServiceMock
                .Setup(x => x.DeleteLoginAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _loginController.DeleteLoginByIdAsync(loginId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }
    }
}
