using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.EnrollmentDTOs
{
    public class ReadEnrollmentDto : BaseEnrollmentDto
    {
        public Guid EnrollmentTableId { get; set; }
        public Guid CourseId { get; set; }
    }
}
