using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Security;
using EduCredit.Service.DTOs.UserDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task< GetUserInfoDto?> GetUserInfoAsync(string? userId, string? userRole)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(userRole))
                return null;

            Person? user = userRole switch
            {
                AuthorizationConstants.AdminRole => await GetUserByRoleAsync<Admin>(userId),
                AuthorizationConstants.TeacherRole => await GetUserByRoleAsync<Teacher>(userId),
                AuthorizationConstants.StudentRole => await GetUserByRoleAsync<Student>(userId),
                _ => await GetUserByRoleAsync<Person>(userId) 
            };

            if (user == null)
                return null;

            var userDto = _mapper.Map<GetUserInfoDto>(user);
            return userDto;
        }
        private async Task<Person?> GetUserByRoleAsync<T>(string userId) where T : Person
        {
            if (!Guid.TryParse(userId, out var userGuid))
                return null; 

            return await _unitOfWork.Repository<T>().GetByIdAsync(userGuid);
        }



    }
}
