using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;
using Microsoft.Extensions.Configuration;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IntegrationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public IntegrationController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        
        [HttpPost("SuccessfulTransaction")]
        [ApiExplorerSettings(IgnoreApi = true)] // hide endpoint

        public async Task<IActionResult> SuccessfulTransaction(
        [FromBody] TransactionDto transaction,
        [FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            var validKey = _configuration["ApiKeys:PaymentApiKey"];
            if (apiKey != validKey)
                return Unauthorized("Invalid API key.");

            // Save transaction
            await _userService.AddPaymentAsync(transaction);
            return Ok("Transaction saved successfully.");
        }
    }
}