using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentApi.Models;
using PaymentApi.Services;

namespace PaymentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("UserCreate")]
        public async Task<IActionResult> UserCreate([FromBody] User dto)
        {
            await _userService.CreateAsync(dto);
            return Ok();
        }

        [HttpDelete("UserDelete")]
        public async Task<IActionResult> UserDelete(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPut("UserUpdate")]
        public async Task<IActionResult> UserUpdate(int id, [FromBody] User dto)
        {
            await _userService.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpPut("DecreaseBalance")]
        public async Task<IActionResult> DecreaseBalance(int id, decimal amount)
        {
            await _userService.DecreaseBalanceAsync(id, amount);
            return Ok();
        }

        [HttpPut("IncreaseBalance")]
        public async Task<IActionResult> IncreaseBalance(int id, decimal amount)
        {
            await _userService.IncreaseBalanceAsync(id, amount);
            return Ok();
        }

        [HttpGet("GetAllTransacitons")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _userService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("GetAllPaymentRequests")]
        public async Task<IActionResult> GetAllPaymentRequests()
        {
            var paymentRequests = await _userService.GetAllPaymentRequestsAsync();
            return Ok(paymentRequests);
        }
    }
}