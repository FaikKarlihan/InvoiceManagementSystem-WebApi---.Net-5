using Microsoft.AspNetCore.Mvc;
using PaymentApi.Models.DTOs;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace PaymentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private static string _lastToken;
        private readonly PaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PaymentController(PaymentService paymentService, HttpClient httpClient, IConfiguration configuration)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost("initiate")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> InitiatePayment([FromBody] InitiatePaymentDto dto,
        [FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            var validKey = _configuration["ApiKeys:PaymentApiKey"];
            if (apiKey != validKey)
                return Unauthorized("Invalid API key.");
            
            var token = await _paymentService.InitiatePaymentAsync(dto);
            _lastToken = token;
            return Ok();
        }

        [HttpGet("GetPaymentToken")]
        public IActionResult GetPaymentToken()
        {
            if (string.IsNullOrEmpty(_lastToken))
                return NotFound("No payment has been initiated yet.");

            return Ok(_lastToken);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentDto dto)
        {
            var transaction = await _paymentService.ConfirmPaymentAsync(dto);

            var url = _configuration["WebApi:BaseUrl"] + "/api/Integration/SuccessfulTransaction";
 
            _httpClient.DefaultRequestHeaders.Remove("X-Api-Key"); // varsa kaldır
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _configuration["ApiKeys:PaymentApiKey"]); // yeniden ekle
            
            var response = await _httpClient.PostAsJsonAsync(url, transaction);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Status: {response.StatusCode}, Body: {content}");

            _lastToken = null;
            return Ok("Successful transaction.");
        }
    }
}
