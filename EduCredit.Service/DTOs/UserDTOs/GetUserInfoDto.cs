using EduCredit.Core.Enums;
namespace EduCredit.Service.DTOs.UserDTOs
{
    public class GetUserInfoDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public DateOnly BirthDate  { get; set; }
        public string Address { get; set; }
        public string NationalId { get; set; }
        
    }
}
