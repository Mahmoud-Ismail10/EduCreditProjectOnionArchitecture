using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Specifications.CourseSpecifications;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class CourseServices : ICourseServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateCourseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
        {
            Course course = _mapper.Map<CreateCourseDto, Course>(createCourseDto);
            await _unitOfWork.Repository<Course>().CreateAsync(course);
            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;
            return createCourseDto;
        }

        public async Task<ApiResponse> DeleteCourseAsync(Guid id)
        {
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
            if (course == null) return new ApiResponse(404);

            await _unitOfWork.Repository<Course>().Delete(course);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        public IReadOnlyList<ReadCourseDto?> GetAllCourses(CourseSpecificationParams specParams, out int count)
        {
            var spec = new CourseWithDeptAndPrevCourseSpecification(specParams);
            var courses = _unitOfWork.Repository<Course>().GetAllSpecification(spec, out count);
            if (courses is null) return null;
            return _mapper.Map<IReadOnlyList<Course>, IReadOnlyList<ReadCourseDto>>(courses);
        }

        public async Task<ReadCourseDto?> GetCourseByIdAsync(Guid id)
        {
            var spec = new CourseWithDeptAndPrevCourseSpecification(id);
            var course = await _unitOfWork.Repository<Course>().GetByIdSpecificationAsync(spec);
            if (course is null) return null;
            return _mapper.Map<Course, ReadCourseDto>(course);
        }
        
        public async Task<ApiResponse> UpdateCourseAsync(UpdateCourseDto updateCourseDto, Guid id)
        {
            var spec = new CourseWithDeptAndPrevCourseSpecification(id);
            var course = await _unitOfWork.Repository<Course>().GetByIdSpecificationAsync(spec);

            if (course is null) return new ApiResponse(404);
            var newCourse = _mapper.Map(updateCourseDto, course);
            await _unitOfWork.Repository<Course>().Update(newCourse);
            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }
    }
}
