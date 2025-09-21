using System;

namespace WebApi.Entities
{
    public class Dues
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }

        public int HousingId { get; set; }
        public Housing Housing { get; set; }
    }
}