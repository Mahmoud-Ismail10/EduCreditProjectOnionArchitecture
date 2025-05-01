using EduCredit.Core;
using EduCredit.Core.Models;
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
                var startDate = currentSemester.EnrollmentOpen;
                var endDate = currentSemester.EnrollmentClose;

                if (now < startDate)
                    return;

                if (now >= endDate)
                {
                    var enrollmentTablesToDelete = await _unitOfWork._enrollmentTableRepo
                        .GetEnrollmentTablesArePendingOrRejectedAsync(currentSemester.Id);

                    if (enrollmentTablesToDelete.Any())
                    {
                        await _unitOfWork.Repository<EnrollmentTable>().DeleteRange(enrollmentTablesToDelete);
                        int result = await _unitOfWork.CompleteAsync();
                        if (result > 0)
                            _logger.LogInformation("Cleaning the enrollment tables has been completed.");
                        else
                            _logger.LogError("Error during cleaning the enrollment tables!");
                    }

                    _timer?.Change(Timeout.Infinite, 0);
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
