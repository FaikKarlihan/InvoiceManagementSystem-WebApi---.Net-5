using System.Collections.Generic;

namespace WebApi.Dtos
{
    public class HousingWithInvoicesViewModel
    {
        public HousingDetailViewModel Housings { get; set; }
        public List<InvoiceDetailViewModel> Invoices { get; set; }
    }
}