using AutoMapper;
using ChargingStations.Application.ChargingStations;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.Shared;
using ChargingStations.Domain.TenantAggregate;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChargingStations.Application.Tenants
{
    public class TenantService : ITenantService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TenantService> _logger;

        public TenantService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<TenantService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<TenantDto>> GetAsync()
        {
            var endpoint = $"endpoint GET /Tenants";
            try
            {
                _logger.LogInformation($"Entered {endpoint}");
                var tenants = await _unitOfWork.TenantRepository.GetAsync();

                var tenantDtos = _mapper.Map<List<Tenant>, List<TenantDto>>(tenants);

                _logger.LogInformation($"Exited {endpoint} with: 200 OK");
                return tenantDtos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exited {endpoint} with: Exception {e}");
                throw;
            }
        }

        public async Task<TenantDto> GetAsync(int tenantId)
        {
            var endpoint = $"endpoint GET /Tenants/{tenantId}";
            try
            {
                _logger.LogInformation($"Entered {endpoint}");
                var tenant = await _unitOfWork.TenantRepository.GetAsync(tenantId);

                var tenantDto = _mapper.Map<Tenant, TenantDto>(tenant);

                _logger.LogInformation($"Exited {endpoint} with: 200 OK");
                return tenantDto;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exited {endpoint} with: Exception {e}");
                throw;
            }
        }

        public async Task<List<ChargingStationDto>> GetChargingStationsAsync(int tenantId)
        {
            var endpoint = $"endpoint GET /Tenants/{tenantId}/ChargingStations";
            try
            {
                _logger.LogInformation($"Entered {endpoint}");
                var tenant = await GetAsync(tenantId);

                if (tenant == null)
                {
                    _logger.LogInformation($"Exited {endpoint} with: 404 Tenant with Id {tenantId} not found");
                    return null;
                }

                var chargingStations = await _unitOfWork.ChargingStationRepository.GetAsync();

                if (chargingStations == null)
                    return new List<ChargingStationDto>();

                chargingStations = chargingStations.Where(x => x.TenantId == tenant.Id).ToList();

                var chargingStationDtos = _mapper.Map<List<ChargingStation>, List<ChargingStationDto>>(chargingStations);

                _logger.LogInformation($"Exited {endpoint} with: 200 OK");
                return chargingStationDtos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exited {endpoint} with: Exception {e}");
                throw;
            }
            
        }
    }
}
