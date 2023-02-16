namespace KWRWebShopTests.Repositories
{
    public class LoginRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly LoginRepository _loginRepository;

        public LoginRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "LoginRepositoryTests")
                .Options;

            _context = new(_options);

            _loginRepository = new(_context);
        }

        [Fact]
        public async void GetAllLoginAsync_ShouldReturnListOfLogins_WhenLoginsExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Login.Add(new Login
            {
                LoginId = 1,
                Email = "mail_1@test.dk",
                Type = 0,
                Password = "123456789"
            });

            _context.Login.Add(new Login
            {
                LoginId = 2,
                Email = "mail_2@test.dk",
                Type = Role.User,
                Password = "123456789"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _loginRepository.GetAllLoginAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Login>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void CreateLoginAsync_ShouldAddNewIdToLogin_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedNewId = 1;

            Login login = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
                Password = "123456789"
            };

            // Act
            var result = await _loginRepository.CreateLoginAsync(login);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Login>(result);
            Assert.Equal(expectedNewId, result?.LoginId);
        }

        [Fact]
        public async void CreateLoginAsync_ShouldFailToAddNewLogin_WhenLoginAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Login login = new()
            {
                Email = "mail_1@test.dk",
                Type = 0,
                Password = "123456789"
            };

            await _loginRepository.CreateLoginAsync(login);

            // Act
            async Task action() => await _loginRepository.CreateLoginAsync(login);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void FindLoginByIdAsync_ShouldReturnLogin_WhenLoginExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int loginId = 1;

            _context.Login.Add(new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = 0,
                Password = "123456789"
            });

            await _context.SaveChangesAsync();

            // Act

            var result = await _loginRepository.FindLoginByIdAsync(loginId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Login>(result);
            Assert.Equal(loginId, result.LoginId);
        }

        [Fact]
        public async void FindLoginByIdAsync_ShouldReturnNull_WhenLoginDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _loginRepository.FindLoginByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateLoginById_ShouldChangeValuesOnLogin_WhenLoginExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int loginId = 1;

            Login newLogin = new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = 0,
                Password = "123456789"
            };
            _context.Login.Add(newLogin);
            await _context.SaveChangesAsync();

            Login updatedLogin = new()
            {
                LoginId = loginId,
                Email = "ny_mail_1@test.dk",
                Type = Role.User,
                Password = "1111111111"
            };

            // Act
            var result = await _loginRepository.UpdateLoginById(loginId, updatedLogin);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Login>(result);
            Assert.Equal(loginId, result?.LoginId);
            Assert.Equal(updatedLogin.Email, result?.Email);
            Assert.Equal(updatedLogin.Type, result?.Type);
            Assert.Equal(updatedLogin.Password, result?.Password);
        }

        [Fact] 
        public async void UpdateLoginById_ShouldReturnNull_WhenLoginDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int loginId = 1;

            Login updatedLogin = new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = 0,
                Password = "123456789"
            };

            // Act
            var result = await _loginRepository.UpdateLoginById(loginId, updatedLogin);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteLoginById_ShouldReturnDeleteHero_WhenLoginIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int loginId = 1;
            Login newLogin = new()
            {
                LoginId = loginId,
                Email = "mail_1@test.dk",
                Type = 0,
                Password = "123456789"
            };

            _context.Login.Add(newLogin);
            await _context.SaveChangesAsync();

            // Act
            var result = await _loginRepository.DeleteLoginByIdAsync(loginId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Login>(result);
            Assert.Equal(loginId, result.LoginId);
        }

        [Fact]
        public async void DeleteLoginById_ShouldReturnNull_WhenLoginDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _loginRepository.DeleteLoginByIdAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
