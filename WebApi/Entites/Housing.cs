using WebApi.Common;
using System.ComponentModel.DataAnnotations;

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
 
        public PlanType PlanType { get; set; }
        public ApartmentStatus ApartmentStatus { get; set; }
    }
}