using EduCredit.Service.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.AdminDTOs
{
    public class ReadAdminDto: BaseRegisterDto
    {
        [JsonPropertyOrder(1)]
        public Guid Id { get; set; }
    }
}
