using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.ChatDTOs
{
    public class ReadMessageDto
    {
        public Guid SenderId { get; set; }
        public string? SenderName { get; set; }
        public string Message { get; set; }
        public DateTime? SendAt { get; set; }
        public Guid CourseId { get; set; }
        public string? CourseName { get; set; }
    }
}
