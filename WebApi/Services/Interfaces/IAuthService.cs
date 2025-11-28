using System.Threading.Tasks;
using WebApi.Dtos;

namespace WebApi.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> AuthenticateAsync(string mail, string password);
    }
}
