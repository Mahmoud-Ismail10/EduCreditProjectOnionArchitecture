using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Chat
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Person Sender { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public string Message { get; set; }
        public DateTime SendAt { get; set; }
    }

}
