namespace EduCredit.Service.DTOs.DepartmentDTOs
{
    public class ReadDepartmentDto : BaseDepartmentDto
    {
        public Guid Id { get; set; }
        public Guid? DepartmentHeadId { get; set; }
        public string DepartmentHeadFullName { get; set; }
    }
}
