namespace WebApi.Common
{
    // Users
    public enum Role
    {
        Admin = 0,
        User = 1
    }

    // Housing 
    public enum ApartmentStatus
    {
        Vacant = 0,
        Occupied = 1
    }
    public enum PlanType
    {
        OnePlusOne = 0,
        TwoPlusOne = 1,
        ThreePlusOne = 2
    }

    // Invoice
    public enum InvoiceType
    {
        Dues = 1,
        Bill = 2
    }
    public enum PaymentStatus
    {
        NotPaid = 1,
        Paid = 2
    }
    public enum OverdueStatus
    {
        NotOverdue = 1,
        Overdue = 2
    }
}