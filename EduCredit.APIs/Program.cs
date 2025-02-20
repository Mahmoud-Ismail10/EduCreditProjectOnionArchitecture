
using EduCredit.APIs.Errors;
using EduCredit.APIs.Extensions;
using EduCredit.APIs.Helper;
using EduCredit.APIs.Middlewares;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
using EduCredit.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduCredit.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSwaggerServices(); // Extension Method
            builder.Services.AddApplicationServices(); // Extension Method
            builder.Services.AddJwtAuthentication(builder.Configuration); // Extension Method
            builder.Services.AddCustomAuthorizationPolicies(); // Extension Method
            /// Connection String
            builder.Services.AddDbContext<EduCreditContext>(Options =>
            Options.UseSqlServer(builder.Configuration.GetConnectionString("CS")));

            var app = builder.Build();

            app.LoggerMiddleWare(); // Extension Method

            app.UseMiddleWares(); // Extension Method

            app.Run();
        }
    }
}
