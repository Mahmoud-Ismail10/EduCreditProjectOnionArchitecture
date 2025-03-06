using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.AuthDTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; }
    }
}
