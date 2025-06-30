using EduCredit.Core.Enums;
using EduCredit.Service.DTOs.NotificationDTOs;
using EduCredit.Service.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    //public class NotificationServices : INotificationServices
    //{
    //    private readonly IHubContext<NotificationHub> _hubContext;

    //    public NotificationServices(IHubContext<NotificationHub> hubContext)
    //    {
    //        _hubContext = hubContext;
    //    }

    //    public async Task SendNotificationToStudentAsync(Status? status, string? GuideNotes, Guid? studentId)
    //    {
    //        if (status == null || studentId == null)
    //            return;

    //        var payload = new
    //        {
    //            Status = status.ToString(),
    //            GuideNotes = GuideNotes ?? string.Empty
    //        };

    //        await _hubContext.Clients.User(studentId.ToString()).SendAsync("EnrollmentStatusUpdated", payload);
    //    }

    //    public async Task SendNotificationToTeacherAsync(string studentName, Guid? teacherId)
    //    {
    //        if (string.IsNullOrWhiteSpace(studentName) || teacherId == null)
    //            return;

    //        string message = $"{studentName} has updated enrollments. You can now review them.";
    //        await _hubContext.Clients.User(teacherId.ToString()).SendAsync("StudentEnrollmentChanged", message);
    //    }
    //}
    public class NotificationServices : INotificationServices
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IInMemoryNotificationStore _notificationStore;

        public NotificationServices(IHubContext<NotificationHub> hubContext, IInMemoryNotificationStore notificationStore)
        {
            _hubContext = hubContext;
            _notificationStore = notificationStore;
        }

        public async Task SendNotificationToStudentAsync(Status? status, string? guideNotes, Guid? studentId)
        {
            if (status == null || studentId == null)
                return;

            var payload = new
            {
                Status = status.ToString(),
                GuideNotes = guideNotes ?? string.Empty
            };

            var notification = new NotificationDto
            {
                Title = "Enrollment Status Updated",
                Message = $"Your enrollment has been updated to '{status}'. {guideNotes}",
                ReceiverId = studentId.Value.ToString(),
                ReceiverType = NotificationReceiverType.Student
            };
            _notificationStore.AddNotification(notification);

            await _hubContext.Clients.User(studentId.ToString())
                .SendAsync("EnrollmentStatusUpdated", payload);
        }

        public async Task SendNotificationToTeacherAsync(string studentName, Guid? teacherId)
        {
            if (string.IsNullOrWhiteSpace(studentName) || teacherId == null)
                return;

            string message = $"{studentName} has updated enrollments. You can now review them.";

            var notification = new NotificationDto
            {
                Title = "Student Enrollment Changed",
                Message = message,
                ReceiverId = teacherId.Value.ToString(),
                ReceiverType = NotificationReceiverType.Teacher
            };
            _notificationStore.AddNotification(notification);

            await _hubContext.Clients.User(teacherId.ToString())
                .SendAsync("StudentEnrollmentChanged", message);
        }
    }


}
