using EduCredit.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.NotificationDTOs
{
    public class NotificationDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ReceiverId { get; set; }
        public NotificationReceiverType ReceiverType { get; set; }
    }
}
