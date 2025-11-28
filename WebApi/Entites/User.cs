using System.ComponentModel.DataAnnotations;
using WebApi.Common;

namespace WebApi.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(11)]
        public string NationalId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Mail { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        public string NumberPlate { get; set; } = ("-");
        public string PasswordHash  { get; set; }
        public Role Role { get; set; } = Role.User;

        public int? HousingId  { get; set; }
        public Housing Housing { get; set; }
    }
}