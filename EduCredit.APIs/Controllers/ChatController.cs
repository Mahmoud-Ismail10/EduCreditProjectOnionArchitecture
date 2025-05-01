using EduCredit.Core.Chat;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduCredit.APIs.Controllers
{
    public class ChatController : BaseApiController
    {
        private readonly ICourseGroupService _groupService;

        public ChatController(ICourseGroupService groupService)
        {
            _groupService = groupService;
        }

        [Authorize]
        [HttpGet("{courseId}/messages")]
        public async Task<IActionResult> GetMessages(string courseId)
        {
            if (!Guid.TryParse(courseId, out Guid parsedCourseId))
                return BadRequest("Invalid course ID");

            var messages = await _groupService.GetMessagesForCourse(parsedCourseId);
            return Ok(messages);
        }

        [Authorize]
        [HttpPost("messages")]
        public async Task<IActionResult> PostMessage([FromBody] ChatMessage message)
        {
            if (message == null || message.CourseId == Guid.Empty || message.SenderId == Guid.Empty || string.IsNullOrWhiteSpace(message.Message))
            {
                return BadRequest("Invalid message data.");
            }

            message.SendAt = DateTime.UtcNow;
            await _groupService.SaveMessageAsync(message);
            return Ok();
        }

        [Authorize]
        [HttpGet("groups/{userId}")]
        public async Task<IActionResult> GetUserGroups(string userId)
        {
            if (!Guid.TryParse(userId, out Guid parsedUserId))
                return BadRequest("Invalid user ID");

            var groups = await _groupService.GetUserCourseGroups(parsedUserId);
            return Ok(groups);
        }
    }
}
