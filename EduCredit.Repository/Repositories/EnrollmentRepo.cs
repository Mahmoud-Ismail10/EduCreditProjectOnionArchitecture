using EduCredit.Core.Relations;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
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
    }
}
