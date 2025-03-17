using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Specifications.AdminSpecifications;
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
        public  IReadOnlyList<ReadAdminDto?> GetAllAdmins(AdminSpecificationParams specParams, out int count)
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

    }
}
