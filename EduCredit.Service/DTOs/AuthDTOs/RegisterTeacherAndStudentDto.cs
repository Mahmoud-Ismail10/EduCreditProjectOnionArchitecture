using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class RegisterStudentAndTeacherDto : BaseRegisterDto
    {
        [Required]
        public Guid DepartmentId { get; set; }
    }
}
