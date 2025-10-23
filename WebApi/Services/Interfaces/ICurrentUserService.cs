namespace WebApi.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string Email { get; }
    }
}