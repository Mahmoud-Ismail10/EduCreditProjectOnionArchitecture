using EduCredit.APIs.Errors;
using EduCredit.APIs.Filters;
using EduCredit.APIs.Helper;
using EduCredit.APIs.Middlewares;
using EduCredit.Core.Models;
using EduCredit.Core;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Core.Security;
using EduCredit.Repository;
using EduCredit.Repository.Data;
using EduCredit.Repository.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using EduCredit.Repository.Data.Identity;
using EduCredit.Core.Services.Contract;
using EduCredit.Service.Services;

namespace EduCredit.APIs.Extensions
{
    public static class ApplicationServicesExtension // static class
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) // Make it Extension Method (this)
        {
            #region Dependancy Injection
            // Add services to the container.
            services.AddControllers();
            services.AddIdentity<Person, IdentityRole<Guid>>()
             .AddEntityFrameworkStores<EduCreditContext>()
             .AddDefaultTokenProviders();

            services.AddScoped<RoleSeeder>();
            services.AddScoped<UserSeeder>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();

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
            services.AddSwaggerGen(op =>
            {
                op.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EduCredit",
                    Version = "v1",
                    Description = "Gradution Project"
                });
                op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.Http,
                    Name = "Authentication",
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Description = "Please enter a valid token"
                });
                op.OperationFilter<SecurityRequirementsOperationFilter>();


            });
            return services;
        }
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            var jwtSettings = configuration.GetSection("JwtSettings");
            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["ValidIssuer"],

                    ValidateAudience = true,
                    ValidAudience = jwtSettings["ValidAudience"],

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),

                    RequireExpirationTime = true,
                };
            });
        }
        public static void AddCustomAuthorizationPolicies(this IServiceCollection service)
        {
            service.AddAuthorization(op =>
            {
                op.AddPolicy(AuthorizationConstants.SuperAdminPolicy, op => op.RequireRole(AuthorizationConstants.SuperAdminRole));
                op.AddPolicy(AuthorizationConstants.AdminPolicy, op => op.RequireRole(AuthorizationConstants.AdminRole));
                op.AddPolicy(AuthorizationConstants.TeacherPolicy, op => op.RequireRole(AuthorizationConstants.TeacherRole));
                op.AddPolicy(AuthorizationConstants.StudentPolicy, op => op.RequireRole(AuthorizationConstants.StudentRole));
            });
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
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 401)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(
                        System.Text.Json.JsonSerializer.Serialize(new ApiResponse(401, "Unauthorized !"))
                    );
                }
            });
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
                    var roleSeeder = services.GetRequiredService<RoleSeeder>();
                    await roleSeeder.AddRolesAsync();
                    var UserSeeder = services.GetRequiredService<UserSeeder>();
                    await UserSeeder.AddSuperAdminAsync();
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
