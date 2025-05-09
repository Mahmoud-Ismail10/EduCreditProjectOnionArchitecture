using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface INotificationServices
    {
        Task SendNotificationToTeacherAsync(string StudentName,Guid? TeacherId);

    }
}
