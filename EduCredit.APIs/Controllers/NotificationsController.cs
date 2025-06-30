using EduCredit.Core.Enums;
using EduCredit.Core.Security;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduCredit.APIs.Controllers
{

    [Authorize]
    public class NotificationsController : BaseApiController
    {
        private readonly IInMemoryNotificationStore _notificationStore;

        public NotificationsController(IInMemoryNotificationStore notificationStore)
        {
            _notificationStore = notificationStore;
        }

        [HttpGet]
        public IActionResult GetNotifications()
        {
            var userId = User.FindFirstValue("userId");
            var role = User.FindFirstValue("role");

            if (userId is null || role is null)
                return Unauthorized(new ApiResponse(401, "Unauthorized: Invalid user"));

            NotificationReceiverType receiverType;

            if (role == AuthorizationConstants.TeacherRole)
            {
                receiverType = NotificationReceiverType.Teacher;
            }
            else if (role == AuthorizationConstants.StudentRole)
            {
                receiverType = NotificationReceiverType.Student;
            }
            else
            {
                return Forbid();
            }

            var notifications = _notificationStore.GetNotifications(userId, receiverType);
            return Ok(notifications);
        }
    }
}
