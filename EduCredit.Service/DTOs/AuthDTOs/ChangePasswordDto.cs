namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class ChangePasswordDto
    {
        public string user_Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
