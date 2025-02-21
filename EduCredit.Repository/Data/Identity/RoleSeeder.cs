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
    public static class RoleSeeder
    {
        public static async Task AddRolesAsync(RoleManager<IdentityRole<Guid>> _roleManager)
        {
            if (_roleManager.Roles.Count() == 0)
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
                    var result = await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });

                    if (!result.Succeeded)
                        throw new Exception($"Failed to add role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

    }
}
