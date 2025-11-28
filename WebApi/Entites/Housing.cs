using WebApi.Common;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public class Housing
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Block { get; set; }

        [Required]
        [MaxLength(2)]
        public string Floor { get; set; }

        [Required]
        [MaxLength(5)]
        public string ApartmentNumber { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public List<Invoice> Invoices { get; set; } = new();

        public PlanType PlanType { get; set; }
        public ApartmentStatus ApartmentStatus
        {
            get
            {
                if (UserId == null)
                    return ApartmentStatus.Vacant;
                else
                    return ApartmentStatus.Occupied;
            }
            private set { }
        }
        public bool? IsOwner { get; set; }
    }
}