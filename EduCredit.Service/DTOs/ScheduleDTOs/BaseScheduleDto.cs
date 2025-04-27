using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.ScheduleDTOs
{
    public class BaseScheduleDto
    {
      
        public Day? Day { get; set; } // enum
        public TimeOnly? LectureStart { get; set; }
        public TimeOnly? LectureEnd { get; set; }
        public string? LectureLocation { get; set; }
    }
}
