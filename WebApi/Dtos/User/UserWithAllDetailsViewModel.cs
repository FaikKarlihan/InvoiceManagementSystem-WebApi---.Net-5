using System.Collections.Generic;

namespace WebApi.Dtos
{
    public class UserWithAllDetailsViewModel
    {
        public UserDetailViewModel User { get; set; }
        public HousingDetailViewModel Housing { get; set; }
        public List<InvoiceDetailViewModel> Invoices { get; set; }
    }
}