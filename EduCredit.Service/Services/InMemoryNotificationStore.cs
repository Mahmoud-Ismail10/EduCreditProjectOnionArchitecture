using EduCredit.Core.Enums;
using EduCredit.Service.DTOs.NotificationDTOs;
using EduCredit.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class InMemoryNotificationStore : IInMemoryNotificationStore
    {
        private readonly List<NotificationDto> _notifications = new();

        public void AddNotification(NotificationDto notification)
        {
            _notifications.Add(notification);
        }

        public List<NotificationDto> GetNotifications(string? receiverId, NotificationReceiverType type)
        {
            return _notifications
                .Where(n => n.ReceiverId == receiverId && n.ReceiverType == type)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();
        }
    }

}
