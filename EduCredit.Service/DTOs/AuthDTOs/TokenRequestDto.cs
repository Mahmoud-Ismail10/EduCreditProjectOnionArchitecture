namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class TokenRequestDto
    {
        public Guid UserId { get; set; }

        public string email { get; set; }

        public string Role { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}
