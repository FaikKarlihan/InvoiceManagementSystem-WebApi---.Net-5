using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHousingService _housingService;
        private readonly IInvoiceService _invoiceService;

        public AdminController(IUserService userService,
                                IHousingService housingService,
                                IInvoiceService invoiceService)
        {
            _userService = userService;
            _housingService = housingService;
            _invoiceService = invoiceService;
        }

        // Basic operations--------------------

        [HttpPost("HousingCreate")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Basic-" })]
        public async Task<IActionResult> HousingCreate([FromBody] HousingCreateDto dto)
        {
            await _housingService.HousingCreateAsync(dto);
            return Ok("Housing has been created successfully.");
        }

        [HttpPost("UserCreate")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Basic-" })]
        public async Task<IActionResult> UserCreate([FromBody] UserCreateDto dto)
        {
            var rawPassword = await _userService.UserCreateAsync(dto);
            return Ok(rawPassword);
        }

        [HttpPut("HousingAssign")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Basic-" })]
        public async Task<IActionResult> AssignHousing(string mail, string apartmentNumber, bool isOwner)
        {
            await _userService.AssignHousingAsync(mail, apartmentNumber, isOwner);
            return Ok("The user has been successfully assigned.");
        }

        [HttpPost("InvoiceCreate")]
        [SwaggerOperation(Description = "This operation will update any existing dues for the entered month/year. A dues fee can exist for every month/year.",
        Tags = new[] { "Admin Operations-Basic-" })]
        public async Task<IActionResult> InvoiceCreate([FromBody] InvoiceCreateDto dto)
        {
            await _invoiceService.InvoiceCreateAsync(dto);
            return Ok("The invoice has been created successfully.");
        }

        [HttpPost("AssignDuesToAll")]
        [SwaggerOperation(Description = "This operation does not affect housings that already have dues for the entered month/year.",
        Tags = new[] { "Admin Operations-Basic-" })]
        public async Task<IActionResult> CreateDuesForAll([FromBody] DuesCreateDto dto)
        {
            await _invoiceService.AssignDuesToAllAsync(dto);
            return Ok("Dues were successfully assigned.");
        }
        
        [HttpGet("GetMessages")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Basic-" })]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _userService.GetAllMessagesAsync();
            return Ok(messages);
        }
        
        // Basic operations--------------------


        // ---------------Extras---------------


        // User--------------------

        [HttpPut("UserUpdate")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-User-" })]
        public async Task<IActionResult> UserUpdate([FromBody] UserUpdateDto dto, string userMail)
        {
            await _userService.UserUpdateAsync(dto, userMail);
            return Ok("User updated successfully.");
        }

        [HttpDelete("UserDelete")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-User-" })]
        public async Task<IActionResult> UserDelete(string userMail)
        {
            await _userService.UserDeleteAsync(userMail);
            return Ok("The user was deleted successfully.");
        }

        [HttpGet("GetAllUsers")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-User-" })]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("GetUser")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-User-" })]
        public async Task<IActionResult> GetUser(string userMail)
        {
            var user = await _userService.GetUserDetailAsync(userMail);
            return Ok(user);
        }

        [HttpGet("GetUserWithHousing")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-User-" })]
        public async Task<IActionResult> GetUserWithHousing(string userMail)
        {
            var user = await _userService.GetUserWithHousingAsync(userMail);
            return Ok(user);
        }

        [HttpGet("GetUserWithInvoices")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-User-" })]
        public async Task<IActionResult> GetUserWithInvoices(string userMail)
        {
            var user = await _userService.GetUserWithInvoicesAsync(userMail);
            return Ok(user);
        }

        [HttpGet("GetUserWithAllDetails")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-User-" })]
        public async Task<IActionResult> UserWithAllDetails(string userMail)
        {
            var user = await _userService.GetUserWithAllDetailsAsync(userMail);
            return Ok(user);
        }

        // User--------------------


        // Housing-----------------

        [HttpPut("HousingUpdate")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Housing-" })]
        public async Task<IActionResult> HousingUpdate([FromBody] HousingUpdateDto dto, string apartmentNumber)
        {
            await _housingService.HousingUpdateAsync(dto, apartmentNumber);
            return Ok("Housing updated successfully.");
        }

        [HttpDelete("HousingDelete")]
        [SwaggerOperation(Description = "All invoices related to housing will be deleted!",Tags = new[] { "Admin Operations-Housing-" })]
        public async Task<IActionResult> HousingDelete(string apartmentNumber)
        {
            await _housingService.HousingDeleteAsync(apartmentNumber);
            return Ok("Housing was deleted successfully.");
        }

        [HttpPut("HousingUnassign")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Housing-" })]
        public async Task<IActionResult> HousingUnassign(string apartmentNumber)
        {
            await _housingService.UnassignFromHousingAsync(apartmentNumber);
            return Ok("The assignment was removed successfully.");
        }

        [HttpGet("GetHousing")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Housing-" })]
        public async Task<IActionResult> GetHousing(string apartmentNumber)
        {
            var housing = await _housingService.GetHousingDetailAsync(apartmentNumber);
            return Ok(housing);
        }

        [HttpGet("GetHousingWithInvoices")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Housing-" })]
        public async Task<IActionResult> GetHousingWithInvoices(string apartmentNumber)
        {
            var housing = await _housingService.GetHousingWithInvoicesAsync(apartmentNumber);
            return Ok(housing);
        }

        [HttpGet("GetAllHousings")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Housing-" })]
        public async Task<IActionResult> GetAllHousings()
        {
            var housings = await _housingService.GetAllHousingsAsync();
            return Ok(housings);
        }

        // Housing-----------------


        // Invoice-----------------   

        [HttpPut("InvoiceUpdate")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Invoice-" })]
        public async Task<IActionResult> InvoiceUpdate([FromBody] InvoiceUpdateDto dto, int id)
        {
            await _invoiceService.InvoiceUpdateAsync(dto, id);
            return Ok("Invoice updated successfully.");
        }

        [HttpDelete("InvoiceDelete")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Invoice-" })]
        public async Task<IActionResult> InvoiceDelete(int id)
        {
            await _invoiceService.InvoiceDeleteAsync(id);
            return Ok("The invoice was deleted successfully.");
        }

        [HttpGet("GetInvoice")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Invoice-" })]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetInvoiceDetailAsync(id);
            return Ok(invoice);
        }

        [HttpGet("GetInvoicesByType")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Invoice-" })]
        public async Task<IActionResult> GetInvoicesByType(InvoiceType? type, bool all = false)
        {
            var invoices = await _invoiceService.GetInvoicesByTypeAsync(type, all);
            return Ok(invoices);
        }

        [HttpGet("GetInvoiceWithAllDetails")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Invoice-" })]
        public async Task<IActionResult> GetInvoiceWithAllDetails(int id)
        {
            var invoice = await _invoiceService.GetWithAllDetailsAsync(id);
            return Ok(invoice);
        }

        [HttpGet("GetAllPayments")]
        [SwaggerOperation(Tags = new[] { "Admin Operations-Invoice-" })]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _userService.GetAllPaymentsAsync();
            return Ok(payments);
        }     
    }
}