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
            CreateEnrollmentDto? enrollment = validationContext.ObjectInstance as CreateEnrollmentDto;
            var courseServices = validationContext.GetService(typeof(ICourseServices)) as ICourseServices;
            var course = courseServices.GetCourseByIdAsync(enrollment.CourseId).Result;
            float Degree = course.CreditHours * 100;

            float Grade = float.Parse(value.ToString()!);
            if (value != null)
                if (Degree >= Grade)
                    return ValidationResult.Success!;

            return new ValidationResult($"Grade must be less than or equal to {Degree}");
        }
    }
}
