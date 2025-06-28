using EduCredit.Core;
using EduCredit.Core.Chat;
using EduCredit.Core.Models;
using EduCredit.Service.Services.Contract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class CleanupService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CleanupService> _logger;
        private Timer _timer;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); // Check every hour

        public CleanupService(IServiceProvider serviceProvider, ILogger<CleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckSemesterAndCleanUp, null, TimeSpan.Zero, _checkInterval);
            return Task.CompletedTask;
        }

        private void CheckSemesterAndCleanUp(object state)
        {
            _ = CheckSemesterAndCleanUpAsync();
        }

        private async Task CheckSemesterAndCleanUpAsync()
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();

                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                // Get Current Semester
                var currentSemester = await _unitOfWork._semesterRepo.GetCurrentSemester();

                if (currentSemester == null)
                    return;

                var now = DateTime.Now;
                var enrollmentStartDate = currentSemester.EnrollmentOpen;
                var enrollmentEndDate = currentSemester.EnrollmentClose;

                var semesterStartDate = currentSemester.StartDate;
                var semesterEndDate = currentSemester.EndDate;

                /// If the period of enrollment is ended, delete all pending or rejected enrollment tables
                if (now >= enrollmentEndDate) // update later
                {
                    var _enrollmentTableServices = scope.ServiceProvider.GetRequiredService<IEnrollmentTableServices>();

                    int result = await _enrollmentTableServices.EnrollmentTablesToDeleteAsync();
                    if (result > 0)
                        _logger.LogInformation("Cleaning the enrollment tables has been completed.");
                    else if (result == 0)
                        _logger.LogWarning("No enrollment table has been cleaned.");
                    else
                        _logger.LogError("Error during cleaning the enrollment tables.");
                }

                /// If the semester is ended, delete all course groups and chat messages
                if (DateOnly.FromDateTime(now) == semesterEndDate) // update later
                {
                    var _courseGroupServices = scope.ServiceProvider.GetRequiredService<ICourseGroupService>();

                    int result = await _courseGroupServices.DeleteAllGroupsAsync();
                    if (result > 0)
                        _logger.LogInformation("Cleaning the course groups has been completed.");
                    else if (result == 0)
                        _logger.LogWarning("No course groups have been cleaned.");
                    else
                        _logger.LogError("Error during cleaning the course groups.");

                    var allChatMessages = await _unitOfWork.Repository<ChatMessage>().GetAllAsync();
                    if (allChatMessages != null && allChatMessages.Any())
                    {
                        await _unitOfWork.Repository<ChatMessage>().DeleteRange(allChatMessages);
                        int chatResult = await _unitOfWork.CompleteAsync();
                        if (chatResult > 0)
                            _logger.LogInformation($"Found {allChatMessages.Count} chat messages to clean.");
                        else if (chatResult == 0)
                            _logger.LogWarning("No chat messages were cleaned, possibly none existed.");
                        else
                            _logger.LogError("Failed to clean chat messages.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking the semester and cleaning up.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
