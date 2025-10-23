namespace WebApi.Dtos
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }     
        public string NationalId { get; set; }
        public string PhoneNumber { get; set; }
        public string NumberPlate { get; set; }
        public string Role { get; set; }
    }
}