using System.ComponentModel.DataAnnotations;

namespace EduCredit.APIs.DTOs.AuthDTOs
{
    public class RegisterTeacherAndStudentDto : BaseRegisterDto
    {
        [Required]
        public Guid DepartmentId { get; set; }
    }
}
