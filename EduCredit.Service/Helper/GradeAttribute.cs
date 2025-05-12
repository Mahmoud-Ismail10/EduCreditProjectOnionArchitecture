using EduCredit.Core.Relations;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.DTOs.EnrollmentDTOs;
using EduCredit.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Helper
{
    public class GradeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Grade is required");

            UpdateEnrollmentDto? enrollment = validationContext.ObjectInstance as UpdateEnrollmentDto;
            var courseServices = validationContext.GetService(typeof(ICourseServices)) as ICourseServices;
            var course = courseServices.GetCourseByIdAsync(enrollment.CourseId).Result;
            float MaxDegree = course.CreditHours * 100;

            float Grade = float.Parse(value.ToString()!);
            if (Grade < 0)
                return new ValidationResult("Grade cannot be negative");
            if (Grade > MaxDegree)
                return new ValidationResult($"Grade must be less than or equal to {MaxDegree}");

            return ValidationResult.Success!;
        }
    }
}
