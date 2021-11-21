using AutoMapper;
using ChargingStations.Application.ChargingStations;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.Shared;
using ChargingStations.Domain.TenantAggregate;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<TenantDto> GetAsync(int tenantId)
        {
            var tenant = await _unitOfWork.TenantRepository.GetAsync(tenantId);
            return _mapper.Map<Tenant, TenantDto>(tenant);
        }

        public async Task<List<ChargingStationDto>> GetChargingStationsAsync(int tenantId)
        {
            var tenant = await GetAsync(tenantId);

            if (tenant == null)
                return null;

            var chargingStations = await _unitOfWork.ChargingStationRepository.GetAsync();

            if (chargingStations == null)
                return new List<ChargingStationDto>();

            chargingStations = chargingStations.Where(x => x.TenantId == tenant.Id).ToList();
            return _mapper.Map<List<ChargingStation>, List<ChargingStationDto>>(chargingStations);
        }
    }
}
