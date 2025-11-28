using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] LoginRequest request)
        {
            var response = await _authService.AuthenticateAsync(request.Mail, request.Password);
            return Ok(response); // access token
        }
    }
}
