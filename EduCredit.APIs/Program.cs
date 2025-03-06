using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
using EduCredit.Repository.Repositories;
using EduCredit.Service.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

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
            ///Redis Connection
            var RedisConnection = builder.Configuration.GetConnectionString("Redis");
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(RedisConnection));

            /// Connection String
            builder.Services.AddDbContext<EduCreditContext>(Options =>
            Options.UseSqlServer(builder.Configuration.GetConnectionString("CS")));

            var app = builder.Build();

            await app.LoggerMiddleWare(); // Extension Method

            app.UseMiddleWares(); // Extension Method

            app.Run();
        }
    }
}
