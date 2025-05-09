using EduCredit.Service.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public class NotificationServices : INotificationServices
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationServices(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendNotificationToTeacherAsync(string StudentName,Guid? TeacherId)
        {
            string message = $"{StudentName} has updated enrollments, You can now see his enrollments .";
            await _hubContext.Clients.User(TeacherId.ToString()).SendAsync("ReceiveNotification", message);

        }
    }
}
