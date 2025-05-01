using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Chat
{
    public class UserCourseGroup
    {
        public Guid UserId { get; set; }
        public Person User { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}
