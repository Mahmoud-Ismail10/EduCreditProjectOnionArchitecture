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
    public class UserSeeder
    {
        private readonly UserManager<Person> _userManager;
        private readonly ILogger<UserSeeder> _logger;

        public UserSeeder(UserManager<Person> userManager, ILogger<UserSeeder> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task AddSuperAdminAsync()
        {
            string superAdminEmail = "Toqae003@gmail.com";
            string superAdminPassword = "SuperAdmin@123";

            var superAdmin = await _userManager.FindByEmailAsync(superAdminEmail);
            if (superAdmin == null)
            {
                superAdmin = new Person
                {
                    UserName = "Toqaessam",
                    Email = superAdminEmail,
                    PhoneNumber = "01006993596",
                    Address = "Menofia",
                    FullName = "Toqa essam",
                    EmailConfirmed = true,
                    Gender = Gender.Female,
                    BirthDate= new DateOnly(2003, 1, 27),
                    NationalId = "30301271701882"
                };

                var createResult = await _userManager.CreateAsync(superAdmin, superAdminPassword);
                if (!createResult.Succeeded)
                {
                    _logger.LogError("Failed to create SuperAdmin: {Errors}",
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    return;
                }

                var roleResult = await _userManager.AddToRoleAsync(superAdmin, AuthorizationConstants.SuperAdminRole);
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("Failed to assign SuperAdmin role: {Errors}",
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
                else
                {
                    _logger.LogInformation("SuperAdmin created and assigned successfully.");
                }
            }
            else
            {
                _logger.LogInformation("SuperAdmin already exists.");
            }
        }
    }
}
