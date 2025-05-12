using EduCredit.Core.Enums;
using EduCredit.Service.DTOs.ScheduleDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.SemesterDTOs
{
    public class ReadSemesterDto : BaseSemesterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SemesterType SemesterType { get; set; }
        public ICollection<ReadScheduleDto> Schedules { get; set; } = new HashSet<ReadScheduleDto>();
    }
}
