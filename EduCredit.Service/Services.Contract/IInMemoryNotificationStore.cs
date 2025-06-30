using EduCredit.Core.Enums;
using EduCredit.Service.DTOs.NotificationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface IInMemoryNotificationStore
    {
        void AddNotification(NotificationDto notification);
        List<NotificationDto> GetNotifications(string? receiverId, NotificationReceiverType type);
    }

}
