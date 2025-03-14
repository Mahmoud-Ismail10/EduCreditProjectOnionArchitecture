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

        public async Task<ApiResponse> SendEmailAsync(string email, string ConfirmEmailUrl, EmailType emailType)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    /// (1) Start connection with host and send port
                    await client.ConnectAsync(_email.Host, _email.Port, true);
                    /// (2) Send Credentials
                    await client.AuthenticateAsync(_email.Email, _email.Password); 

                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress(_email.From, _email.Email)); // Display Name, Email Address of source
                    emailMessage.To.Add(MailboxAddress.Parse(email)); // Email Address of destination
                    emailMessage.Subject = emailType switch // The text that will be pressed (Provided Link)
                    {
                        EmailType.ConfirmEmail => "Confirm Your Email",
                        EmailType.ForgotPassword => "Reset Password",
                        _ => "Email Confirmation"
                    };

                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = emailType switch
                        {
                            EmailType.ConfirmEmail => $"<p>Please confirm your email by clicking the link below:</p> <br> <a href=\"{ConfirmEmailUrl}\" style=\"color:blue; text-decoration:underline;\">Confirm Email</a>",
                            EmailType.ForgotPassword => $"<p>Please reset your password by clicking the link below:</p> <br> <a href=\"{ConfirmEmailUrl}\" style=\"color:blue; text-decoration:underline;\">Reset Password</a>",
                            _ => "<p>Please confirm your email using the provided link.</p>"
                        },
                        TextBody = emailType switch
                        {
                            EmailType.ConfirmEmail => $"Please confirm your email using the provided link: {ConfirmEmailUrl}",
                            EmailType.ForgotPassword => $"Please reset your password using the provided link: {ConfirmEmailUrl}",
                            _ => "Please confirm your email using the provided link."
                        }
                    };
                    emailMessage.Body = bodyBuilder.ToMessageBody(); // Body of message

                    /// (3) Send content of message
                    await client.SendAsync(emailMessage);
                    /// (4) Close Connection
                    await client.DisconnectAsync(true); 
                }
                return new ApiResponse(200);
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, $"Error sending email: {ex.Message}");
            }

        }
       
    }
}
