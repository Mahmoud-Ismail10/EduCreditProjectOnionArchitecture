using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduCredit.Core.Enums;
using EduCredit.Core.Security;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using MailKit.Net.Smtp;
using MimeKit;

namespace EduCredit.Service.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailSetting _email;

        public EmailServices(EmailSetting email)
        {
            _email = email;
        }
        public async Task<ApiResponse> SendEmailAsync(string email, string ConfirmEmailUrl,EmailType emailType)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_email.Host, _email.Port, false);
                    await client.AuthenticateAsync(_email.Email, _email.Password);

                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("From : ", _email.Email));
                    emailMessage.To.Add(new MailboxAddress("To : ", email));
                    emailMessage.Subject = emailType switch
                    {
                        EmailType.ConfirmEmail => "Confirm Your Email",
                        EmailType.ForgotPassword => "Reset Password",
                        _ => "Email Confirmation"
                    };
                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = emailType switch
                        {
                            EmailType.ConfirmEmail => $"<p>Please confirm your email by clicking the link below:</p> <a href='{ConfirmEmailUrl}'>Confirm Email</a>",
                            EmailType.ForgotPassword => $"<p>Please reset your password by clicking the link below:</p> <a href='{ConfirmEmailUrl}'>Reset Password</a>",
                            _ => "Please confirm your email using the provided link."
                        },
                        TextBody = emailType switch
                        {
                            EmailType.ConfirmEmail => "Please confirm your email using the provided link.",
                            EmailType.ForgotPassword => "Please reset your password using the provided link.",
                            _ => "Please confirm your email using the provided link."
                        }
                    };


                    emailMessage.Body = bodyBuilder.ToMessageBody();

                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
                return new ApiResponse(200, "Email sent successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, $"Error sending email: {ex.Message}");
            }

        }
       
    }
}
