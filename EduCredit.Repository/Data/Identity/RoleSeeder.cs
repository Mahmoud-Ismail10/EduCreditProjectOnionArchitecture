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
    public class RoleSeeder
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ILogger<RoleSeeder> _logger;


        public RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager, ILogger<RoleSeeder> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task AddRolesAsync()
        {
            string[] roles =
            {
                AuthorizationConstants.AdminRole,
                AuthorizationConstants.SuperAdminRole,
                AuthorizationConstants.StudentRole,
                AuthorizationConstants.TeacherRole
            };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });

                    if (result.Succeeded)
                        _logger.LogInformation($"Role '{role}' has been added successfully.");
                    else
                        _logger.LogError($"Failed to add role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

    }
}
