using AutoMapper;
using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ChargingStations.Application.Chargers
{
    public class ChargerService : IChargerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChargerService> _logger;

        public ChargerService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<ChargerService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ChargerDto> GetAsync(int chargerId)
        {
            var endpoint = $"endpoint GET /ChargerModels";
            try
            {
                _logger.LogInformation($"Entered {endpoint}");
                var charger = await _unitOfWork.ChargerRepository.GetAsync(chargerId);
                var chargingStation = await _unitOfWork.ChargingStationRepository.GetAsync(charger.ChargingStationId);

                if (charger.Id == 0 || chargingStation == null)
                {
                    _logger.LogInformation($"Exited {endpoint} with: 404 Charger with Id {chargerId} not found");
                    return null;
                }

                var chargerDto = _mapper.Map<Charger, ChargerDto>(charger);
                chargerDto.ChargingStationName = chargingStation.Name;
                chargerDto.Address = chargingStation.Address;

                var chargerModel = await _unitOfWork.ChargerModelRepository.GetAsync(charger.ChargerModelId);
                chargerDto.ModelName = chargerModel.Name;
                chargerDto.Manufacturer = chargerModel.Manufacturer;

                _logger.LogInformation($"Exited {endpoint} with: 200 OK");
                return chargerDto;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exited {endpoint} with: Exception {e}");
                throw;
            }
        }
    }
}
