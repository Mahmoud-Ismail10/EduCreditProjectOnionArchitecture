using EduCredit.Core.Models;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Repositories
{
    public class TeacherRepo : GenericRepository<Teacher>, ITeacherRepo
    {
        private readonly EduCreditContext _dbcontext;

        public TeacherRepo(EduCreditContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }
    }
}
