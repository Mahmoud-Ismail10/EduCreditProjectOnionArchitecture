using EduCredit.Core.Models;
using EduCredit.Core.Relations;
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
    public class EnrollmentRepo : GenericRepository<Enrollment>, IEnrollmentRepo
    {
        private readonly EduCreditContext _dbcontext;

        public EnrollmentRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Enrollment?> GetEnrollmentByIdsAsync(Guid enrollmentTableId, Guid courseId)
        {
            Enrollment? enrollment = await _dbcontext.Enrollments
                .FirstOrDefaultAsync(c => c.EnrollmentTableId == enrollmentTableId && c.CourseId == courseId);
            return enrollment;
        }
    }
}
