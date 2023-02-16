using KWRWebShopAPI.Authorization;

namespace KWRWebShopTests.Services
{
    public class LoginServiceTests
    {
        private readonly LoginService _loginService;
        private readonly Mock<ILoginRepository> _loginRepositoryMock = new();
        private readonly IJwtUtils _jwtUtils;

        public LoginServiceTests() 
        {
            _loginService = new(_loginRepositoryMock.Object, _jwtUtils);
        }

        [Fact]
        public async void GetAllLoginAsync_ShouldReturnListOfLoginResponses_WhenLoginsExists()
        {
            // Arrange
            List<Login> logins = new()
            {
                new()
                {
                LoginId = 1,
                Email = "mail_1@test.dk",
                Type = Role.User,
                Password = "123456789"
                },
                new()
                {
                LoginId = 2,
                Email = "mail_2@test.dk",
                Type = Role.Admin,
                Password = "123456789"
                }
            };

            _loginRepositoryMock
                .Setup(x => x.GetAllLoginAsync())
                .ReturnsAsync(logins);

            // Act
            var result = await _loginService.GetAllLoginAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<LoginResponse>>(result);
            Assert.Equal(2, logins.Count);
        }

        [Fact]
        public async void GetAllLoginAsync_ShouldReturnEmptyListOfLoginResponses_WhenNoLoginsExists()
        {
            // Arrange
            List<Login> logins = new();

            _loginRepositoryMock
                .Setup(x => x.GetAllLoginAsync())
                .ReturnsAsync(logins);

            // Act
            var result = await _loginService.GetAllLoginAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<LoginResponse>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetAllLoginAsync_ShouldThrowNullException_WhenRepositoryReturnsNull()
        {
            // Arrange
            List<Login> logins = new();

            _loginRepositoryMock
                .Setup(x => x.GetAllLoginAsync())
                .ReturnsAsync(() => throw new ArgumentNullException());

            // Act
            async Task action() => await _loginService.GetAllLoginAsync();

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);
        }

        [Fact]
        public async void CreateLoginAsync_ShouldReturnLoginResponse_WhenCreateIsSuccess()
        {
            // Arrange
            LoginRequest newLogin = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
            };

            int LoginId = 1;
            Login login = new()
            {   
                LoginId = LoginId,
                Email = "mail_1@test.dk",
                Type = 0,
                Password = "123456789"
            };

            _loginRepositoryMock
                .Setup(x => x.CreateLoginAsync(It.IsAny<Login>()))
                .ReturnsAsync(login);

            // Act
            var result = await _loginService.CreateLoginAsync(newLogin);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<LoginResponse>(result);
            Assert.Equal(login.LoginId, result.LoginId);
            Assert.Equal(login.Email, result.Email);
            Assert.Equal(login.Type, result.Type);
        }

        [Fact]
        public async void CreateLoginAsync_ShouldThrowNullExeption_WhenRepositioryReturnsNull()
        {
            // Arrange
            LoginRequest newLogin = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
            };

            _loginRepositoryMock
                .Setup(x => x.CreateLoginAsync(It.IsAny<Login>()))
                .ReturnsAsync(() => null);

            // Act
            async Task action() => await _loginService.CreateLoginAsync(newLogin);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);
        }

        [Fact] 
        public async void FindLoginByIdAsync_ShouldReturnLoginResponse_WhenLoginExists()
        {
            // Arrange
            int loginId = 1;

            Login login = new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = 0,
            };

            _loginRepositoryMock
                .Setup(x => x.FindLoginByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(login);

            // Act
            var result = await _loginService.FindLoginByIdAsync(loginId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<LoginResponse>(result);
            Assert.Equal(login.LoginId, result.LoginId);
            Assert.Equal(login.Email, result.Email);
            Assert.Equal(login.Type, result.Type);
        }

        [Fact]
        public async void FindLoginByIdAsync_ShouldReturnNull_WhenLoginDoesNotExist()
        {
            // Arrange
            int loginId = 1;

            _loginRepositoryMock
                .Setup(x => x.FindLoginByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _loginService.FindLoginByIdAsync(loginId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateLoginByIdAsync_ShouldReturnLoginResponse_WhenUpdateIsSuccess()
        {
            // Arrange
            LoginRequest loginRequest = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
            };

            int loginId = 1;
            Login login = new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = 0,
            };

            _loginRepositoryMock
                .Setup(x => x.UpdateLoginById(It.IsAny<int>(), It.IsAny<Login>()))
                .ReturnsAsync(login);

            // Act
            var result = await _loginService.UpdateLoginAsync(loginId, loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<LoginResponse>(result);
            Assert.Equal(login.LoginId, result.LoginId);
            Assert.Equal(login.Email, result.Email);
            Assert.Equal(login.Type, result.Type);
        }

        [Fact]
        public async void UpdateLoginById_ShouldReturnNull_WhenLoginDoesNotExist()
        {
            // Arrange
            LoginRequest loginRequest = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
            };

            int loginId = 1;

            _loginRepositoryMock
                .Setup(x => x.UpdateLoginById(It.IsAny<int>(), It.IsAny<Login>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _loginService.UpdateLoginAsync(loginId, loginRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteLoginById_ShouldReturnLoginResponse_WhenDeleteIsSuccess()
        {
            // Arrange
            int loginId = 1;

            Login login = new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = 0,
            };

            _loginRepositoryMock
                .Setup(x => x.DeleteLoginByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(login);

            // Act
            var result = await _loginService.DeleteLoginAsync(loginId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<LoginResponse>(result);
            Assert.Equal(loginId, result.LoginId);
        }

        [Fact]
        public async void DeleteLoginByIdAsync_ShouldReturnNull_WHenLoginDoesNotExist()
        {
            // Arrange
            int loginId = 1;

            _loginRepositoryMock
                .Setup(x => x.DeleteLoginByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _loginService.DeleteLoginAsync(loginId);

            // Assert
            Assert.Null(result);
        }
    }
}
