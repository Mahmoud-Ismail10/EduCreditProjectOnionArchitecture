using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Specifications.AdminSpecifications;
using EduCredit.Core.Specifications.CourseSpecifications;
using EduCredit.Core.Specifications.EnrollmentsSpecifications;
using EduCredit.Core.Specifications.EnrollmentTableSpecifications;
using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Core.Specifications.TeacherSpecefications;
using EduCredit.Service.DTOs.AdminDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;

namespace EduCredit.Service.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IReadOnlyList<ReadAdminDto?> GetAllAdmins(AdminSpecificationParams specParams, out int count)
        {
            var adminRepo = _unitOfWork.Repository<Admin>();
            var spec = new AdminSpecification(specParams);
            var admins = adminRepo.GetAllSpecification(spec, out count);
            if (admins is null)
                return null;
            var AdminsDto = _mapper.Map<IReadOnlyList<Admin>, IReadOnlyList<ReadAdminDto>>(admins);
            return AdminsDto;
        }

        public async Task<ReadAdminDto?> GetAdminByIdAsync(Guid id)
        {
            var adminRepo = _unitOfWork.Repository<Admin>();
            var spec = new AdminSpecification(id);
            var admin = await adminRepo.GetByIdSpecificationAsync(spec);
            if (admin is null) return null;
            return _mapper.Map<Admin, ReadAdminDto>(admin);
        }

        public async Task<UpdateAdminDto?> UpdateAdminAsync(UpdateAdminDto updateadminDto, Guid id)
        {
            var spec = new AdminSpecification(id);
            var adminFromDB = await _unitOfWork.Repository<Admin>().GetByIdSpecificationAsync(spec);
            if (adminFromDB is null) return null;

            var newAdmin = _mapper.Map(updateadminDto, adminFromDB);
            await _unitOfWork.Repository<Admin>().Update(newAdmin);

            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return _mapper.Map<Admin, UpdateAdminDto>(newAdmin); throw new NotImplementedException();
        }
        public async Task<ApiResponse> DeleteAdminAsync(Guid id)
        {
            var admin = await _unitOfWork.Repository<Admin>().GetByIdAsync(id);
            if (admin is null) return new ApiResponse(404);

            await _unitOfWork.Repository<Admin>().Delete(admin);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        public async Task<StatisticsDto> GetStatistics(Statistics statistics, Guid? departmentId)
        {
            if ((statistics == Statistics.DepartmentSuccessRate || statistics == Statistics.DepartmentTotalCourses
                || statistics== Statistics.DepartmentTotalStudents
                || statistics== Statistics.DepartmentTotalTeachers
                )
                && (departmentId == null || departmentId == Guid.Empty))
            {
                return new StatisticsDto
                {
                    Type = "In this case you must send department Id !"
                    ,Total=-1
                };
            }
            var total = statistics switch
            {
                Statistics.Students => await _unitOfWork.Repository<Student>().CountAsync(),
                Statistics.Teachers => await _unitOfWork.Repository<Teacher>().CountAsync(),
                Statistics.Courses => await _unitOfWork.Repository<Course>().CountAsync(),
                Statistics.Departments => await _unitOfWork.Repository<Department>().CountAsync(),
                Statistics.SuccessRate => await GetSuccessRateAsync(),
                Statistics.DepartmentSuccessRate => await GetSuccessRateAsync(departmentId),
                Statistics.DepartmentTotalCourses => await _unitOfWork.Repository<Course>().CountAsync(new CourseWithDeptAndPrevCourseSpecification(departmentId)),
                Statistics.DepartmentTotalStudents => await _unitOfWork.Repository<Student>().CountAsync(new StudentWithDepartmentAndGuideSpecification(departmentId)),
                Statistics.DepartmentTotalTeachers => await _unitOfWork.Repository<Teacher>().CountAsync(new TeacherWithDepartmentSpecifications(departmentId)),
                _ => 0
            };

            if (!Enum.IsDefined(typeof(Statistics), statistics))
            {
                return new StatisticsDto
                {
                    Type = "Invalid",
                    Total = 0
                };
            }
            return new StatisticsDto
            {
                Type = statistics.ToString(),
                Total = total
            };
        }
        private async Task<double> GetSuccessRateAsync(Guid? departmentId = null)
        {
            var spec = new EnrollmentsWithCoursesSpecification(departmentId, passedOnly: true);

            int totalPassed = await _unitOfWork
                .Repository<Enrollment>()
                .CountAsync(spec);

            var enrolledSpec = new EnrollmentsWithCoursesSpecification(departmentId);

            int totalEnrolled = await _unitOfWork
                .Repository<Enrollment>()
                .CountAsync(enrolledSpec);

            if (totalEnrolled == 0)
                return 0;

            return Math.Round((double)totalPassed / totalEnrolled * 100, 2);
        }



    }
}
