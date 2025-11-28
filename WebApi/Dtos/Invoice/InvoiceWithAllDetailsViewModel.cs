namespace WebApi.Dtos
{
    public class InvoiceWithAllDetailsViewModel
    {
        public InvoiceDetailViewModel Invoices { get; set; }  
        public UserDetailViewModel User { get; set; }
        public HousingDetailViewModel Housing { get; set; } 
    }
}