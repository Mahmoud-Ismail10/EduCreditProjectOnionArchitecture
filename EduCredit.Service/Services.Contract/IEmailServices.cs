using EduCredit.Core.Enums;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface IEmailServices
    {
        Task<ApiResponse> SendEmailAsync(string email, string ConfirmEmailUrl,EmailType emailType);
    }
}
