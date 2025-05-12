using EduCredit.Service.DTOs.CourseDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Helper
{
    public class MinDegreeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            BaseCourseDto? course = validationContext.ObjectInstance as BaseCourseDto;
            float MaxDegree = course!.CreditHours * 100;

            float MinDegree = float.Parse(value.ToString()!);
            if (value != null)
            {
                if (MinDegree < 0)
                    return new ValidationResult("Minimum Degree cannot be negative");
                if (MaxDegree < MinDegree)
                    return new ValidationResult($"Minimum Degree must be less than {MaxDegree}");

                return ValidationResult.Success!;
            }
            return new ValidationResult("Grade is required");
        }
    }
}
