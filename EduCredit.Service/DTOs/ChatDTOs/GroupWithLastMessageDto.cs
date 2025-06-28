using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.ChatDTOs
{
    public class GroupWithLastMessageDto
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }

        public string? LastMessageText { get; set; }
        public string? LastMessageSenderName { get; set; }
        public DateTime? LastMessageTime { get; set; }
    }
}
