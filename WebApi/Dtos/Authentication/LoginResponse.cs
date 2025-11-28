namespace WebApi.Dtos
{
    public class LoginResponse
    {
        public string Mail { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
    }
}
