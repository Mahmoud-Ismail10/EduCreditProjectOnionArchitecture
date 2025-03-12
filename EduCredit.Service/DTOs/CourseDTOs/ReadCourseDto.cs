namespace EduCredit.Service.DTOs.CourseDTOs
{
    public class ReadCourseDto : BaseCourseDto
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public string PreviousCourseName { get; set; }
    }
}
