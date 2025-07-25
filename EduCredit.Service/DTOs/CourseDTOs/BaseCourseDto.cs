﻿using EduCredit.Service.Helper;
using System.ComponentModel.DataAnnotations;

namespace EduCredit.Service.DTOs.CourseDTOs
{
    public class BaseCourseDto
    {
        [Required(ErrorMessage = "Course Name is required")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [RegularExpression(@"^[A-Za-z\s]+[1-9]?$", ErrorMessage = "Name must contain only letters and spaces, and optionally a digit 1-9 at the end.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Credit Hours is required")]
        public float CreditHours { get; set; }
        
        [Required(ErrorMessage = "Duration is required")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Minimum Degree is required")]
        [MinDegree] /// Custom Attribute
        public float MinimumDegree { get; set; }
    }
}
