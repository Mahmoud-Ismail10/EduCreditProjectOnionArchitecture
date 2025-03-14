using EduCredit.Service.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.TeacherDTOs
{
    public class ReadTeacherDto : GetUserInfoDto
    {
        [JsonPropertyOrder(1)]
        public Guid Id { get; set; }
        [JsonPropertyOrder(9)]
        public DateOnly AppointmentDate { get; set; }
        [JsonPropertyOrder(10)]
        public Guid? DepartmentId { get; set; }
        [JsonPropertyOrder(11)]
        public string DepartmentFullName { get; set; }
    }
}
