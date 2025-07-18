﻿using EduCredit.Core.Enums;
using EduCredit.Service.DTOs.AuthDTOs;
using EduCredit.Service.DTOs.StudentDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.EnrollmentDTOs
{
    public class ReadEnrollmentDto : BaseEnrollmentDto
    {
        public float? Grade { get; set; }
        public float? Percentage { get; set; }
        public Appreciation? Appreciation { get; set; }
        public bool? IsPassAtCourse { get; set; }
        public  string? CourseName { get; set; }
        public float? CreditHours { get; set; }
    }   
}
