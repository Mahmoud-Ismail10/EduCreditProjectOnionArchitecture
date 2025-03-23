using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Specifications.CourseSpecifications;
using EduCredit.Core.Specifications.SemesterCoursesSpecifications;
using EduCredit.Core.Specifications.SemesterSpecifications;
using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.DTOs.EnrollmentTableDTOs;
using EduCredit.Service.DTOs.SemesterDTOs;
using EduCredit.Service.DTOs.StudentDTOs;
using EduCredit.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class EnrollmentTableServices : IEnrollmentTableServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        //اظهر المواد المتاحه للتسجيل 
        //الشروط
        //1-الطالب خلص السابق للماده
        //2-الطالب لا يسجل الماده من قبل
        //4-الماده متاحه للتسجيل في هذا الترم 
        //5-أن يكون الطالب قد رسب فيها من قب لإن كان قد سجلها سابقًا ولم ينجح 
        public EnrollmentTableServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IReadOnlyList<ReadEnrollmentTableDto>?> GetStudentAvailableCourses(string studentId)
        {
            //get student by id
            var stuId = Guid.Parse(studentId);
            var studentSpec = new StudentWithDepartmentAndGuideSpecification(stuId);
            var student = await _unitOfWork.Repository<Student>().GetByIdSpecificationAsync(studentSpec);
            if (student is null) return null;

            int count;
            var semesterSpec = new SemesterWithCoursesSpecifications();
            var semester = await _unitOfWork.Repository<Semester>().GetByIdSpecificationAsync(semesterSpec);
            if (semester is null) return null;

            var semesterCourses = new CourseWithDeptAndPrevCourseSpecification(semester.Id);
            var courses = _unitOfWork.Repository<Course>().GetAllSpecification(semesterCourses, out count);

            var CourseMapped = mapper.Map<IReadOnlyList<Course>, List<ReadCourseDto>>(courses.ToList());

            var StudentAndavailableCoursesDto = new ReadEnrollmentTableDto()
            {
                StudentFullName = student.FullName,
                Level = student.Level,
                DepartmentName = student.Department.Name is null ? "" : student.Department.Name,
                GPA = student.GPA,
                Obtainedhours = student.CreditHours,
                AvailableHours = student.CreditHours <= 102 ? 18 : 21,
                NameOfGuide = student.Teacher.FullName is null ? "" : student.Teacher.FullName,
                //semesterCourses = CourseMapped.ToList(),
            };
            return new List<ReadEnrollmentTableDto> { StudentAndavailableCoursesDto };

        }




        //3-الطالب لا يسجل الماده في نفس الوقت
        //5-الطالب لا يسجل اكثر من 18 ساعه في الترم الحالي
        //6-الطالب لا يسجل اكثر من 10 ساعات في الصيفي
    }
}
