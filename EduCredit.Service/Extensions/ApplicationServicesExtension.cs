﻿using EduCredit.Core.Models;
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
using EduCredit.Core.Repositories.Contract;
using EduCredit.Repository.Repositories;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Any;
using EduCredit.Service.Hubs;
using EduCredit.Core.Chat;


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
                options.Password.RequiredLength = 8; // Set minimum length to 8 characters
            }).AddEntityFrameworkStores<EduCreditContext>().AddDefaultTokenProviders();
            services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(1);
        });
            /// Add life time for Services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IEnrollmentRepo), typeof(EnrollmentRepo));
            services.AddScoped(typeof(IEnrollmentTableRepo), typeof(EnrollmentTableRepo));
            services.AddScoped(typeof(IScheduleRepo), typeof(ScheduleRepo));
            services.AddScoped(typeof(ITeacherRepo), typeof(TeacherRepo));
            services.AddScoped(typeof(ICourseRepo), typeof(CourseRepo));
            services.AddScoped(typeof(ISemesterRepo), typeof(SemesterRepo));
            services.AddScoped(typeof(IDepartmentRepo), typeof(DepartmentRepo));
            services.AddScoped(typeof(ITeacherScheduleRepo), typeof(TeacherScheduleRepo));
            services.AddScoped(typeof(IStudentRepo), typeof(StudentRepo));
            services.AddScoped(typeof(IChatMessageRepo), typeof(ChatMessageRepo));
            services.AddScoped(typeof(IUserCourseGroupRepo), typeof(UserCourseGroupRepo));

            services.AddScoped(typeof(IEnrollmentServices), typeof(EnrollmentServices));
            services.AddScoped(typeof(IScheduleServices), typeof(ScheduleServices));
            services.AddScoped(typeof(IEnrollmentTableServices), typeof(EnrollmentTableServices));
            services.AddScoped(typeof(IScheduleServices), typeof(ScheduleServices));
            services.AddScoped(typeof(ISemesterServices), typeof(SemesterServices));
            services.AddScoped(typeof(IDepartmentServices), typeof(DepartmentServices));
            services.AddScoped(typeof(ICourseServices), typeof(CourseServices));
            services.AddScoped(typeof(ITeacherServices), typeof(TeacherServices));
            services.AddScoped(typeof(IAdminServices), typeof(AdminServices));
            services.AddScoped(typeof(IStudentServices), typeof(StudentServices));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(ICourseGroupService), typeof(CourseGroupService));

            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailServices, EmailServices>();
            services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
            services.AddScoped<INotificationServices, NotificationServices>();
            /// Hosted Service use for Background Task
            services.AddHostedService<CleanupService>();
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddSingleton<IInMemoryNotificationStore, InMemoryNotificationStore>();
            services.AddSingleton<EmailSetting>(configuration.GetSection(nameof(EmailSetting)).Get<EmailSetting>());
            /// Auto Mapper use parameter less ctor of MappingProfiles
            services.AddAutoMapper(typeof(MappingProfiles));
            /// Add SignalR services
            services.AddSignalR();
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
            var allowedOrigins = configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();
            services.AddCors(Options =>
            {
                Options.AddPolicy("AllowAll",
                    policy => policy
                    //.AllowAnyOrigin() /// Not allow to use AlloeAnyOrigin with AllCredentials
                    //.WithOrigins(allowedOrigins)
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                    //.AllowCredentials()); /// for get user id from claims
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
                op.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time", // دي بتقول للـ Swagger إنه نوع وقت
                    Example = new OpenApiString("14:30:00")
                });

                op.MapType<TimeOnly?>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Nullable = true,
                    Example = new OpenApiString("14:30:00")
                });

            });
            return services;
        }
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["SecretKey"];
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentNullException("JWT SecretKey is missing in configuration.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

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

                    IssuerSigningKey = securityKey,
                    RequireExpirationTime = true,

                    NameClaimType = "email",
                    RoleClaimType = "role",
                };

                op.Events = new JwtBearerEvents
                {
                    /// Support SignalR
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/chathub", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },

                    OnTokenValidated = context =>
                    {
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity != null)
                        {
                            var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
                            var roleClaim = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;

                            claimsIdentity.TryRemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Email));
                            claimsIdentity.TryRemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Role));

                            if (!string.IsNullOrWhiteSpace(emailClaim))
                                claimsIdentity.AddClaim(new Claim("email", emailClaim));

                            if (!string.IsNullOrWhiteSpace(roleClaim))
                                claimsIdentity.AddClaim(new Claim("role", roleClaim));
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("message: Token has expired. Please login again.");
                        }
                        return Task.CompletedTask;
                    },

                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("message : You do not have permission to access this resource.");
                    }
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

            // Map SignalR hubs
            app.MapHub<NotificationHub>("/notificationHub");
            app.MapHub<ChatHub>("/chatHub");
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
