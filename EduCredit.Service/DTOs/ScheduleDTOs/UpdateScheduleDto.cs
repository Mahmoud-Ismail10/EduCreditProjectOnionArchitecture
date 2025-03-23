using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.ScheduleDTOs
{
    public class UpdateScheduleDto : BaseScheduleDto
    {
        public DateOnly? ExamDate { get; set; }
        public TimeOnly? ExamStart { get; set; }
        public TimeOnly? ExamEnd { get; set; }
        public string? ExamLocation { get; set; }
    }
}
