using EduCredit.Core;
using EduCredit.Core.Chat;
using EduCredit.Core.Models;
using EduCredit.Service.DTOs.ChatDTOs;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Identity;
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
        private readonly ICourseGroupService _courseGroupService;

        public ChatHub(IUserService userService, ICourseGroupService courseGroupService)
        {
            _userService = userService;
            _courseGroupService = courseGroupService;
        }

        public override async Task OnConnectedAsync()
        {
            var userGuid = _userService.GetUserGuidFromClaims(Context.User);

            var courseGroups = await _courseGroupService.GetUserCourseGroups(userGuid);
            foreach (var group in courseGroups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group.GroupId.ToString());
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

            var userGroups = await _courseGroupService.GetUserCourseGroups(userGuid);
            if (!userGroups.Any(g => g.GroupId == courseId))
                throw new HubException("Unauthorized to send messages in this course.");

            var newMessageDto = new ReadMessageDto
            {
                SenderId = userGuid,
                CourseId = courseId,
                Message = message
            };

            try
            {
                var preparedMessage = await _courseGroupService.CreateMessageDtoAsync(newMessageDto);
                var savedMessage = await _courseGroupService.SaveMessageAsync(preparedMessage);

                if (savedMessage == null)
                    throw new HubException("Failed to save message.");

                await Clients.Group(courseId.ToString()).SendAsync("ReceiveMessage", savedMessage);
            }
            catch (Exception ex)
            {
                throw new HubException("An error occurred while sending the message.", ex);
            }
        }

        public async Task SendTypingNotification(Guid courseId, string userName)
        {
            var userGuid = _userService.GetUserGuidFromClaims(Context.User);

            var userGroups = await _courseGroupService.GetUserCourseGroups(userGuid);
            if (!userGroups.Any(g => g.GroupId == courseId))
                throw new HubException("Unauthorized to send typing notification in this course.");

            await Clients.Group(courseId.ToString()).SendAsync("UserTyping", userName);
        }
    }
}
