using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Repositories.Contract
{
    public interface IDepartmentRepo
    {
        Task<bool> CheckDepartmentExistingAsync(string name);
    }
}
