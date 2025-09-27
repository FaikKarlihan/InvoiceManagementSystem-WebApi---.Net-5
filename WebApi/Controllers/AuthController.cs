using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Entities;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.AuthenticateAsync(request.Username, request.Password);
            if (response == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(response); // AccessToken returns
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "").Trim();

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is not found.");

            await _authService.LogoutAsync(token);
            return Ok(new {Message = "Log out, token canceled."}); 
        }

    }
}
