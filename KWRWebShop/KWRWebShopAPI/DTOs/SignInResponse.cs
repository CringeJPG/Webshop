namespace KWRWebShopAPI.DTOs
{
    public class SignInResponse
    {
        public string Token { get; set; }
        public LoginResponse LoginResponse { get; set; }
    }
}
