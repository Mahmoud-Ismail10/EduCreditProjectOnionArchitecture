using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.CourseDTOs
{
    public class UpdateCourseDto : BaseCourseDto
    {
        [Required(ErrorMessage = "Department Name is required")]
        public Guid DepartmentId { get; set; }

        public Guid? PreviousCourseId { get; set; }
    }
}
