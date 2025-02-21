using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Data.Identity
{
    public static class UserSeeder
    {
        public static async Task AddSuperAdminAsync(UserManager<Person> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var superAdmin1 = new Person()
                {
                    UserName = "Toqaessam",
                    Email = "Toqae003@gmail.com",
                    PhoneNumber = "01006993596",
                    Address = "Menofia",
                    FullName = "Toqa essam",
                    EmailConfirmed = true,
                    Gender = Gender.Female,
                    BirthDate= new DateOnly(2003, 1, 27),
                    NationalId = "30301271701882"
                };
                var createResult1 = await _userManager.CreateAsync(superAdmin1, superAdmin1.NationalId);
                if (createResult1.Succeeded)
                {
                    var roleResult1 = await _userManager.AddToRoleAsync(superAdmin1, AuthorizationConstants.SuperAdminRole);
                    if (!roleResult1.Succeeded)
                        throw new Exception($"Failed to assign SuperAdmin1 role " +
                            $": {string.Join(", ", roleResult1.Errors.Select(e => e.Description))}");
                }
                else
                {
                    throw new Exception($"Failed to create SuperAdmin1 " +
                        $": {string.Join(", ", createResult1.Errors.Select(e => e.Description))}");
                }


                var superAdmin2 = new Person()
                {
                    FullName = "Mahmoud Ismail",
                    Email = "mahmoud.ismail1872@gmail.com",
                    UserName = "mahmoud.ismail",
                    PhoneNumber = "01002876238",
                    Address = "Menofia",
                    EmailConfirmed = true,
                    BirthDate = new DateOnly(2003, 4, 4),
                    Gender = Gender.Male,
                    NationalId = "30304041701459"
                };
                var createResult2 = await _userManager.CreateAsync(superAdmin2, superAdmin2.NationalId);
                if (createResult2.Succeeded)
                {
                    var roleResult2 = await _userManager.AddToRoleAsync(superAdmin2, AuthorizationConstants.SuperAdminRole);
                    if (!roleResult2.Succeeded)
                        throw new Exception($"Failed to assign SuperAdmin2 role " +
                            $": {string.Join(", ", roleResult2.Errors.Select(e => e.Description))}");
                }
                else
                {
                    throw new Exception($"Failed to create SuperAdmin2 " +
                        $": {string.Join(", ", createResult1.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
