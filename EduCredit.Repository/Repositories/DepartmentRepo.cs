using EduCredit.Core.Models;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Repositories
{
    public class DepartmentRepo : GenericRepository<Department>, IDepartmentRepo
    {
        private readonly EduCreditContext _dbcontext;

        public DepartmentRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<bool> CheckDepartmentExistingAsync(string Name)
        {
            return await _dbcontext.Departments.AnyAsync(d => d.Name.ToLower() == Name.ToLower());
        }
    }
}
