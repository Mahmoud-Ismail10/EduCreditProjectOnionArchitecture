using EduCredit.APIs.Errors;
using EduCredit.APIs.Helper;
using EduCredit.APIs.Middlewares;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Data;
using EduCredit.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduCredit.APIs.Extensions
{
    public static class ApplicationServicesExtension // static class
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) // Make it Extension Method (this)
        {
            #region Dependancy Injection
            // Add services to the container.
            services.AddControllers();

            #region New Services
            /// Auto Mapper use parameter less ctor of MappingProfiles
            services.AddAutoMapper(typeof(MappingProfiles));
            
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            /// Custom Validation Errors
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ContentResult) =>
                {
                    var errors = ContentResult.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                         .SelectMany(p => p.Value.Errors)
                                                         .Select(e => e.ErrorMessage)
                                                         .ToList();
                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            #endregion 
            #endregion
            return services;
        }

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static WebApplication UseMiddleWares(this WebApplication app)
        {
            #region Middlewares
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            /// Use when there is an End Point not exist error and we need to Redirect it to another End Point.
            app.UseStatusCodePagesWithRedirects("/Error");

            app.UseHttpsRedirection();

            app.UseAuthorization();
            /// Used when data contains static files (pictures)  
            //app.UseStaticFiles();

            app.MapControllers();
            #endregion
            return app;
        }

        public static async Task<WebApplication> LoggerMiddleWare(this WebApplication app)
        {
            #region Update Database Automaticaly
            /// Ask CLR Explicitly for creating object from EduCreditContext
            /// Can used 'using' before initial scope instead of try finally
            var scope = app.Services.CreateScope(); // Add Scope
            try
            {
                var services = scope.ServiceProvider;
                var _dbcontext = services.GetRequiredService<EduCreditContext>();
                var loggerfactory = services.GetRequiredService<ILoggerFactory>(); // Custom Exceptions
                try
                {
                    await _dbcontext.Database.MigrateAsync(); // Update Database
                }
                catch (Exception ex)
                {
                    var logger = loggerfactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred during migration!");
                }
            }
            finally
            {
                scope.Dispose(); // Close the connection with the Database
            }
            #endregion
            return app;
        }
    }
}
