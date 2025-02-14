using EduCredit.Core.Repositories.Contract;
using EduCredit.Core.Models;
using EduCredit.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Repositories
{
    public class CourseRepo : GenericRepository<Course>, ICourseRepo
    {
        private readonly EduCreditContext _dbcontext;

        public CourseRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }
    }
}
