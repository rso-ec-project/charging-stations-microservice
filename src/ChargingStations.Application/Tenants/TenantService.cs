using AutoMapper;
using ChargingStations.Domain.Shared;
using ChargingStations.Domain.TenantAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Application.Tenants
{
    public class TenantService : ITenantService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TenantService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TenantDto>> GetAsync()
        {
            var tenants = await _unitOfWork.TenantRepository.GetAsync();
            return _mapper.Map<List<Tenant>, List<TenantDto>>(tenants);
        }

        public async Task<TenantDto> GetAsync(int id)
        {
            var tenant = await _unitOfWork.TenantRepository.GetAsync(id);
            return _mapper.Map<Tenant, TenantDto>(tenant);
        }
    }
}
