﻿using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.DepartmentDTOs
{
    public class ReadDepartmentDto : BaseDepartmentDto
    {
        public Guid Id { get; set; }
        public string DepartmentHeadFullName { get; set; }
    }
}
