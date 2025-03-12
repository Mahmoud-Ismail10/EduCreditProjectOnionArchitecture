using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.CourseDTOs
{
    public class CreateCourseDto : BaseCourseDto
    {
        [Required(ErrorMessage = "Department Name is required")]
        public Guid DepartmentId { get; set; }

        public Guid? PreviousCourseId { get; set; }
    }
}
