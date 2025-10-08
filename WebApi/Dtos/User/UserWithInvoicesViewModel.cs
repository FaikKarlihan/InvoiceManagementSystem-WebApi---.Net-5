using System.Collections.Generic;

namespace WebApi.Dtos
{
    public class UserWithInvoiceViewModel
    {
        public UserDetailViewModel User { get; set; }
        public List<InvoiceDetailViewModel> Invoices { get; set; }
    }
}