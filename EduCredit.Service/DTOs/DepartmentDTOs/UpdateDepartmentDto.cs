using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.DepartmentDTOs
{
    public class UpdateDepartmentDto : BaseDepartmentDto
    {
        public Guid? DepartmentHeadId { get; set; }
    }
}
