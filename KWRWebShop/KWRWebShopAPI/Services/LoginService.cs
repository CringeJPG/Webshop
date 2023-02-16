namespace KWRWebShopAPI.Services
{
    public interface ILoginService
    {
        Task<SignInResponse> AuthenticateUser(SignInRequest login);
        Task<LoginResponse> RegisterAsync(CustomerSignupRequest newUser);
        Task<List<LoginResponse>> GetAllLoginAsync();
        Task<LoginResponse> CreateLoginAsync(LoginRequest newLogin);
        Task<LoginResponse> FindLoginByIdAsync(int loginId);
        Task<LoginResponse?> UpdateLoginAsync(int loginId, LoginRequest updatedLogin);
        Task<LoginResponse?> DeleteLoginAsync(int loginId);
    }
    public class LoginService : ILoginService
    {

        private readonly ILoginRepository _loginRepository;
        private readonly IJwtUtils _jwtUtils;

        public LoginService(ILoginRepository loginRepository, IJwtUtils jwtUtils)
        {
            _loginRepository = loginRepository;
            _jwtUtils = jwtUtils;
        }

        private LoginResponse MapLoginToLoginResponse(Login login)
        {
            if (login.Customer == null)
            {
                return new LoginResponse
                {
                    LoginId = login.LoginId,
                    Email = login.Email,
                    Type = login.Type,
                };
            }
            else {
                return new LoginResponse
                {
                    LoginId = login.LoginId,
                    Email = login.Email,
                    Type = login.Type,
                    
                    Customer = new LoginCustomerResponse
                    {
                        CustomerId = login.Customer.CustomerId,
                        FirstName = login.Customer.FirstName,
                        LastName = login.Customer.LastName,
                        Address = login.Customer.Address,
                        Created = login.Customer.Created
                    }
                }; 
            }
            
        }

        private Login MapLoginRequestToLogin(LoginRequest loginRequest)
        {
            return new Login
            {
                Email = loginRequest.Email,
                Type = loginRequest.Type,
                Password = loginRequest.Password
            };
        }

        private Login MapCustomerSignupRequestToLogin(CustomerSignupRequest customerSignupRequest)
        {
            return new Login
            {
                Email = customerSignupRequest.Email,
                Type = Role.User,
                Password = customerSignupRequest.Password,
                Customer = new()
                {
                    FirstName = customerSignupRequest.Customer.FirstName,
                    LastName = customerSignupRequest.Customer.LastName,
                    Address = customerSignupRequest.Customer.Address
                }
            };
        }

     
        public async Task<SignInResponse> AuthenticateUser(SignInRequest login)
        {
            Login user = await _loginRepository.FindLoginByEmailAsync(login.Email);
            if (user == null)
            {
                return null;
            }

            if (user.Password == login.Password)
            {
                SignInResponse response = new()
                {
                    LoginResponse = new()
                    {
                        LoginId = user.LoginId,
                        Email = user.Email,
                        Type = user.Type,
                        Customer = new() 
                        {
                            CustomerId = user.Customer.CustomerId,
                            FirstName = user.Customer.FirstName,
                            LastName = user.Customer.LastName,
                            Address = user.Customer.Address,
                            Created = user.Customer.Created
                        }

                    },
                    Token = _jwtUtils.GenerateJwtToken(user)
                };
                return response;
            }
            return null;
        }

        public async Task<LoginResponse> RegisterAsync(CustomerSignupRequest newUser)
        {
            var user = await _loginRepository.RegisterAsync(MapCustomerSignupRequestToLogin(newUser));

            if (user == null)
            {
                throw new ArgumentNullException();
            }

            return MapLoginToLoginResponse(user);
        }

        public async Task<List<LoginResponse>> GetAllLoginAsync()
        {
            List<Login> logins = await _loginRepository.GetAllLoginAsync();

            if (logins == null)
            {
                throw new ArgumentNullException();
            }

            return logins.Select(login => MapLoginToLoginResponse(login)).ToList();
        }

        public async Task<LoginResponse> CreateLoginAsync(LoginRequest newLogin)
        {
            var login = await _loginRepository.CreateLoginAsync(MapLoginRequestToLogin(newLogin));

            if (login == null)
            {

                throw new ArgumentNullException();
            }

            return MapLoginToLoginResponse(login);
        }

        public async Task<LoginResponse> FindLoginByIdAsync(int loginId)
        {
            var login = await _loginRepository.FindLoginByIdAsync(loginId);

            if (login != null)
            {
                return MapLoginToLoginResponse(login);
            }

            return null;
        }

        public async Task<LoginResponse?> UpdateLoginAsync(int loginId, LoginRequest updatedLogin)
        {
            var login = await _loginRepository.UpdateLoginById(loginId, MapLoginRequestToLogin(updatedLogin));

            if (login != null)
            {
                return MapLoginToLoginResponse(login);
            }

            return null;
        }

        public async Task<LoginResponse?> DeleteLoginAsync(int loginId)
        {
            var login = await _loginRepository.DeleteLoginByIdAsync(loginId);

            if (login != null)
            {
                return MapLoginToLoginResponse(login);
            }

            return null;
        }
    }
}
