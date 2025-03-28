using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Service.Errors;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.DTOs.EnrollmentTableDTOs;
using EduCredit.Service.DTOs.SemesterDTOs;
using EduCredit.Service.DTOs.StudentDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;

namespace EduCredit.Service.Services
{
    public class EnrollmentTableServices : IEnrollmentTableServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISemesterServices _semesterServices;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
      
        //اظهر المواد المتاحه للتسجيل 
        //الشروط
        //1-الطالب خلص السابق للماده
        //2-الطالب لا يسجل الماده من قبل
        //4-الماده متاحه للتسجيل في هذا الترم 
        //5-أن يكون الطالب قد رسب فيها من قب لإن كان قد سجلها سابقًا ولم ينجح 
        public EnrollmentTableServices(IUnitOfWork unitOfWork, ISemesterServices semesterServices, IMapper mapper, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _semesterServices = semesterServices;
            _mapper = mapper;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateOrUpdateEnrollmentTable(CreateOrUpdateEnrollmentTableDto createOrUpdateEnrollmentTableDto, string studentId)
        {
            if (await _semesterServices.IsEnrollmentOpenAsync())
            {
                createOrUpdateEnrollmentTableDto.Status = Status.Pending;

                var currentSemester = await _unitOfWork._semesterRepo.GetCurrentSemester();
                if (currentSemester is null) return new ApiResponse(404, "There is no current semester!");
                createOrUpdateEnrollmentTableDto.SemesterId = currentSemester.Id;

                var stuId = Guid.Parse(studentId);
                var existEnrollmentTable = await _unitOfWork._enrollmentTableRepo.GetEnrollmentTableByStudentIdAndSemesterIdAsync(stuId, currentSemester.Id);
                /// When the student enrolls the table for the first time in the current semester 
                if (existEnrollmentTable is null)
                {
                    var createEnrollmentTable = _mapper.Map<CreateOrUpdateEnrollmentTableDto, EnrollmentTable>(createOrUpdateEnrollmentTableDto);
                    await _unitOfWork.Repository<EnrollmentTable>().CreateAsync(createEnrollmentTable);

                    int result = await _unitOfWork.CompleteAsync();
                    if (result <= 0) return new ApiResponse(400, "Failed to create enrollment table!");
                    return new ApiResponse(200, "Enrollment Table created successfully");
                }
                /// When the student has already enrolled the table and tries to update it
                else
                {
                    var updateEnrollmentTable = _mapper.Map(createOrUpdateEnrollmentTableDto, existEnrollmentTable);
                    await _unitOfWork.Repository<EnrollmentTable>().Update(updateEnrollmentTable);

                    int result = await _unitOfWork.CompleteAsync();
                    if (result <= 0) return new ApiResponse(400, "Failed to update enrollment table!");
                    return new ApiResponse(200, "Enrollment Table updated successfully");
                }
            }
            return new ApiResponse(400, "Failed to create enrollment table, Because the enrollment period ended!");
        }

        public Task<IReadOnlyList<ReadEnrollmentTableDto>?> GetStudentAvailableCourses(string studentId)
        {
            throw new NotImplementedException();
        }

        //        //3-الطالب لا يسجل الماده في نفس الوقت
        //        //5-الطالب لا يسجل اكثر من 18 ساعه في الترم الحالي
        //        //6-الطالب لا يسجل اكثر من 10 ساعات في الصيفي

    }
}