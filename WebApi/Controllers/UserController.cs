using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUser;

        public UserController(IUserService userService, ICurrentUserService currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        [HttpGet("GetMyDetail")]
        public async Task<IActionResult> GetUserDetail()
        {
            var user = await _userService.GetUserDetailAsync(_currentUser.Email);
            return Ok(user);
        }

        [HttpGet("GetMyHousing")]
        public async Task<IActionResult> GetHousingDetail()
        {
            var housing = await _userService.GetUserHousingDetailAsync(_currentUser.Email);
            return Ok(housing);
        }

        [HttpGet("GetMyInvoices")]
        public async Task<IActionResult> GetInvoicesDetail()
        {
            var invoices = await _userService.GetUserInvoicesAsync(_currentUser.Email);
            return Ok(invoices);
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] UserMessageDto dto)
        {
            await _userService.SendMessageAsync(dto, _currentUser.UserId);
            return Ok("Message sent successfully.");
        }
        
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] string newPassword)
        {
            await _userService.ChangePasswordAsync(newPassword, _currentUser.Email);
            return Ok("The password has been changed successfully.");
        }

        [HttpGet("GetMyPayments")]
        public async Task<IActionResult> GetUserPayments()
        {
            var payments = await _userService.GetUserPaymentsAsync(_currentUser.Email);
            return Ok(payments);
        }      
          
        [HttpPost("MakePayment")]
        public async Task<IActionResult> MakePayment(int InvoiceId)
        {
            await _userService.MakePaymentAsync(InvoiceId, _currentUser.UserId);
            return Ok("Go=> Payment Api.");// dene
        }
    }
}