using EduCredit.Service.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.TeacherDTOs
{
    public class UpdateTeacherDto:BaseRegisterDto
    {
        public DateOnly AppointmentDate { get; set; }
    }
}
