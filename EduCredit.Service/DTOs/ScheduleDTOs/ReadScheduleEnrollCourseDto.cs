using EduCredit.Core.Enums;
using EduCredit.Service.DTOs.StudentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.ScheduleDTOs
{
    public class ReadScheduleEnrollCourseDto : ReadStudentDto
    {
        public List<ReadScheduleDto> SchedualeCourses { get; set; } = new List<ReadScheduleDto>();
    }
}
