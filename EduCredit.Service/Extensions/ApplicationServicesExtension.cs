using EduCredit.Core.Models;
using EduCredit.Core;
using EduCredit.Core.Security;
using EduCredit.Repository;
using EduCredit.Repository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using EduCredit.Core.Services.Contract;
using EduCredit.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using EduCredit.Service.Helper;
using EduCredit.Service.Errors;
using EduCredit.Service.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using EduCredit.Service.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EduCredit.Repository.Data.Identity;
using EduCredit.Service.Services.Contract;
using AspNetCoreRateLimit;


namespace EduCredit.Service.Extensions
{
    public static class ApplicationServicesExtension  // static class
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) // Make it Extension Method (this)
        {
            #region Dependancy Injection
            // Add services to the container.
            services.AddControllers();
            services.AddIdentity<Person, IdentityRole<Guid>>(options =>
            {
                /// Configurations of Password
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<EduCreditContext>().AddDefaultTokenProviders();
                services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);
            });
            /// Add life time for Services
            services.AddScoped<ICacheService,CacheService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IDepartmentServices), typeof(DepartmentServices));
            services.AddScoped(typeof(ITeacherServices), typeof(TeacherServices));
            services.AddScoped(typeof(IUserService), typeof(UserService));

            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(ICourseServices), typeof(CourseServices));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailServices, EmailServices>();
            services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();

            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddSingleton<EmailSetting>(configuration.GetSection(nameof(EmailSetting)).Get<EmailSetting>());
            /// Auto Mapper use parameter less ctor of MappingProfiles
            services.AddAutoMapper(typeof(MappingProfiles));

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

            #region Cors
            services.AddCors(Options =>
            {
                Options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            #endregion

            #region Rate Limit
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>(); 
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            services.Configure<IpRateLimitOptions>(options =>
            {
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Limit = 100,
                        Period = "1m"
                    }
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

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? string.Empty)),

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

            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}
            /// Use when there is an End Point not exist error and we need to Redirect it to another End Point.
            app.UseStatusCodePagesWithRedirects("/Error");

            app.UseHttpsRedirection();
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 401)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(
                        System.Text.Json.JsonSerializer.Serialize(new ApiResponse(401, "Token must be provided in the Authorization header."))
                    );
                }
            });
            app.UseAuthentication();
            app.UseMiddleware<BlackListMiddleware>();
            app.UseAuthorization();
            /// Used when data contains static files (pictures)  
            //app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseIpRateLimiting();
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
                var _roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var _userManager = services.GetRequiredService<UserManager<Person>>();
                var loggerFactory = services.GetRequiredService<ILoggerFactory>(); // Custom Exceptions
                try
                {
                    await _dbcontext.Database.MigrateAsync(); // Update Database
                    await RoleSeeder.AddRolesAsync(_roleManager);
                    await UserSeeder.AddSuperAdminAsync(_userManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger("Migration");
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
