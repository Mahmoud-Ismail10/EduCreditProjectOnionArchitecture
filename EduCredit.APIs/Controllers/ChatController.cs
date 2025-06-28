using EduCredit.Core.Chat;
using EduCredit.Service.DTOs.ChatDTOs;
using EduCredit.Service.Hubs;
using EduCredit.Service.Services;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EduCredit.APIs.Controllers
{
    public class ChatController : BaseApiController
    {
        private readonly ICourseGroupService _courseGroupService;
        private readonly IUserService _userService;

        public ChatController(ICourseGroupService courseGroupService, IUserService userService)
        {
            _courseGroupService = courseGroupService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet("{courseId}/messages")]
        public async Task<IActionResult> GetMessages(string courseId)
        {
            if (!Guid.TryParse(courseId, out Guid parsedCourseId))
                return BadRequest("Invalid course ID");

            var userGuid = _userService.GetUserGuidFromClaims(User);
            var userGroups = await _courseGroupService.GetUserCourseGroups(userGuid);

            if (!userGroups.Any(g => g.GroupId == parsedCourseId))
                return Forbid("You are not authorized to access this course's messages.");

            var messages = await _courseGroupService.GetMessagesForCourse(parsedCourseId);
            return Ok(messages);
        }

        [Authorize]
        [HttpPost("messages")]
        public async Task<IActionResult> PostMessage([FromBody] ReadMessageDto message)
        {
            var userGuid = _userService.GetUserGuidFromClaims(User);

            var userGroups = await _courseGroupService.GetUserCourseGroups(userGuid);
            if (!userGroups.Any(g => g.GroupId == message.CourseId))
                return Forbid("You are not authorized to send messages in this course.");

            message.SenderId = userGuid;

            try
            {
                var preparedMessage = await _courseGroupService.CreateMessageDtoAsync(message);
                var savedMessage = await _courseGroupService.SaveMessageAsync(preparedMessage);

                if (savedMessage == null)
                    return BadRequest("Failed to save message.");

                await _courseGroupService.BroadcastMessageAsync(savedMessage);
                return Ok(savedMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while sending the message: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("groups/{userId}")]
        public async Task<IActionResult> GetUserGroups(string userId)
        {
            if (!Guid.TryParse(userId, out Guid parsedUserId))
                return BadRequest("Invalid user ID");

            var groups = await _courseGroupService.GetUserCourseGroups(parsedUserId);
            return Ok(groups);
        }
    }
}
