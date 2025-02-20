using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Data.IdentitySeeding
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<Person> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var superAdmin = new Admin()
                {
                    FullName = "Mahmoud Ismail",
                    Email = "mahmoud.ismail@gmail.com",
                    UserName = "mahmoud.ismail",
                    PhoneNumber = "1234567890",
                    Address = "Menofia",
                    BirthDate = new DateOnly(2003, 4, 4),
                    Gender = Gender.Male,
                    NationalId = "30304041701458"
                };
                await _userManager.CreateAsync(superAdmin, superAdmin.NationalId);

                //var student = new Student()
                //{
                //    FullName = "ahmed yasser",
                //    Email = "ahmed.yasser@gmail.com",
                //    UserName = "ahmed.yasser",
                //    PhoneNumber = "1234567890",
                //    Address = "Menofia",
                //    BirthDate = new DateOnly(2003, 4, 4),
                //    Gender = Gender.Male,
                //    NationalId = "30304041701458",
                //    CreditHours = 0,
                //    GPA = 0,
                //    Level = 1,
                //    DepartmentId = Guid.Parse("6beca797-806f-47bc-95ca-6b78dee738ea")
                //};
                //await _userManager.CreateAsync(student, student.NationalId);

                var teacher = new Teacher()
                {
                    FullName = "shaimaa salem",
                    Email = "shaimaa.salem@gmail.com",
                    UserName = "shaimaa.salem",
                    PhoneNumber = "1234567890",
                    Address = "Menofia",
                    BirthDate = new DateOnly(2003, 4, 4),
                    Gender = Gender.Female,
                    NationalId = "30304041701458",
                    AppointmentDate = DateOnly.Parse("4/4/2003"),
                    DepartmentId = Guid.Parse("6beca797-806f-47bc-95ca-6b78dee738ea")
                };
                await _userManager.CreateAsync(teacher, teacher.NationalId);
            }
        }
    }
}
