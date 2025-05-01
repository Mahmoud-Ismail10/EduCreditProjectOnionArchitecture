using EduCredit.Core.Chat;
using EduCredit.Core.Models;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EduCredit.Service.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;
        private readonly ICourseGroupService _groupService;

        public ChatHub(IUserService userService, ICourseGroupService groupService)
        {
            _userService = userService;
            _groupService = groupService;
        }

        public override async Task OnConnectedAsync()
        {
            var userGuid = _userService.GetUserGuidFromClaims(Context.User);

            var courseGroups = await _groupService.GetUserCourseGroups(userGuid);
            foreach (var group in courseGroups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToGroup(Guid courseId, string message)
        {
            var userGuid = _userService.GetUserGuidFromClaims(Context.User);

            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderId = userGuid,
                CourseId = courseId,
                Message = message,
                SendAt = DateTime.UtcNow
            };

            await _groupService.SaveMessageAsync(chatMessage);

            await Clients.Group(courseId.ToString()).SendAsync("ReceiveMessage", new
            {
                SenderId = userGuid,
                Message = message,
                Timestamp = chatMessage.SendAt
            });
        }
    }
}
