using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class BaseUserDto : BaseRegisterDto
    {
        public Guid? DepartmentId { get; set; }
    }
}
