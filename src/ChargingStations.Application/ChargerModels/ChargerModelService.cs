using AutoMapper;
using ChargingStations.Domain.ChargerModelAggregate;
using ChargingStations.Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Application.ChargerModels
{
    public class ChargerModelService : IChargerModelService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChargerModelService> _logger;

        public ChargerModelService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<ChargerModelService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<ChargerModelDto>> GetAsync()
        {
            var endpoint = $"endpoint GET /ChargerModels";
            try
            {
                _logger.LogInformation($"Entered {endpoint}");
                var chargerModels = await _unitOfWork.ChargerModelRepository.GetAsync();

                var chargerModelDtos = _mapper.Map<List<ChargerModel>, List<ChargerModelDto>>(chargerModels);

                _logger.LogInformation($"Exited {endpoint} with: 200 OK");
                return chargerModelDtos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exited {endpoint} with: Exception {e}");
                throw;
            }
        }
    }
}
