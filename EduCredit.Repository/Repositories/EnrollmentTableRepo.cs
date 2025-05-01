using EduCredit.Core.Enums;
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
    public class EnrollmentTableRepo : GenericRepository<EnrollmentTableRepo>, IEnrollmentTableRepo
    {
        private readonly EduCreditContext _dbcontext;

        public EnrollmentTableRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<EnrollmentTable?> GetEnrollmentTableByStudentIdAndSemesterIdAsync(Guid? studentId, Guid semesterId)
        {
            var enrollmentTable = await _dbcontext.EnrollmentTables
                .FirstOrDefaultAsync(s => s.StudentId == studentId && s.SemesterId == semesterId);
            return enrollmentTable;
        }
        
        public async Task<IReadOnlyList<EnrollmentTable?>> GetEnrollmentTablesArePendingOrRejectedAsync(Guid semesterId)
        {
            var enrollmentTables = await _dbcontext.EnrollmentTables
                .Where(s => s.Status == Status.Pending && s.Status == Status.Rejected).ToListAsync();
            return enrollmentTables;
        }
    }
}
