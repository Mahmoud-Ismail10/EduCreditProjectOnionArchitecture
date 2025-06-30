using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Chat;
using EduCredit.Core.Models;
using EduCredit.Core.Specifications.CourseSpecifications;
using EduCredit.Service.DTOs.ChatDTOs;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Hubs;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class CourseGroupService : ICourseGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Person> _userManager;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public CourseGroupService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Person> userManager, IHubContext<ChatHub> chatHubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _chatHubContext = chatHubContext;
        }

        public async Task<List<GroupWithLastMessageDto>> GetUserCourseGroups(Guid userId)
        {
            var userGroups = await _unitOfWork._userCourseGroupRepo.GetUserCourseGroupsWithChatMessages(userId);

            var result = userGroups.Select(ug =>
            {
                var lastMessage = ug.Course.ChatMessages
                    .OrderByDescending(m => m.SendAt)
                    .FirstOrDefault();

                return new GroupWithLastMessageDto
                {
                    GroupId = ug.Course.Id,
                    GroupName = ug.Course.Name,
                    LastMessageText = lastMessage?.Message,
                    LastMessageSenderName = lastMessage?.Sender?.FullName,
                    LastMessageTime = lastMessage?.SendAt
                };
            }).ToList();

            return result;
        }

        public async Task<ReadMessageDto> CreateMessageDtoAsync(ReadMessageDto readMessageDto)
        {
            var person = await _userManager.FindByIdAsync(readMessageDto.SenderId.ToString());
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(readMessageDto.CourseId);

            if (person == null || course == null) return null;

            return new ReadMessageDto
            {
                SenderId = readMessageDto.SenderId,
                SenderName = person?.FullName ?? "Unknown",
                Message = readMessageDto.Message,
                //SendAt = DateTime.UtcNow,
                SendAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
                CourseId = readMessageDto.CourseId,
                CourseName = course?.Name ?? "Unnamed Course"
            };
        }

        public async Task<ReadMessageDto?> SaveMessageAsync(ReadMessageDto readMessageDto)
        {
            var message = _mapper.Map<ReadMessageDto, ChatMessage>(readMessageDto);
            await _unitOfWork.Repository<ChatMessage>().CreateAsync(message);
            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return readMessageDto;
        }

        public async Task<IReadOnlyList<ReadMessageDto>> GetMessagesForCourse(Guid courseId)
        {
            var messages = await _unitOfWork._chatMessageRepo.GetMessagesByCourseIdAsync(courseId);
            return _mapper.Map<IReadOnlyList<ChatMessage>, IReadOnlyList<ReadMessageDto>>(messages);
        }

        public async Task AddUserToCourseGroup(Guid userId, Guid courseId)
        {
            if (!await _unitOfWork._userCourseGroupRepo.ExistsAsync(userId, courseId))
            {
                await _unitOfWork.Repository<UserCourseGroup>().CreateAsync(new UserCourseGroup
                {
                    UserId = userId,
                    CourseId = courseId
                });
            }
        }

        public async Task<int> DeleteAllGroupsAsync()
        {
            var allGroups = await _unitOfWork.Repository<UserCourseGroup>().GetAllAsync();
            await _unitOfWork.Repository<UserCourseGroup>().DeleteRange(allGroups);
            int result = await _unitOfWork.CompleteAsync();
            return result;
        }

        public async Task BroadcastMessageAsync(ReadMessageDto messageDto)
        {
            await _chatHubContext.Clients.Group(messageDto.CourseId.ToString()).SendAsync("ReceiveMessage", messageDto);
        }
    }
}