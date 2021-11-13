using AutoMapper;
using ChargingStations.Application.Chargers;
using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChargingStations.Application.ChargingStations
{
    public class ChargingStationService : IChargingStationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ChargingStationService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ChargingStationDto>> GetAsync()
        {
            var chargingStations = await _unitOfWork.ChargingStationRepository.GetAsync();
            return _mapper.Map<List<ChargingStation>, List<ChargingStationDto>>(chargingStations);
        }

        public async Task<ChargingStationDto> GetAsync(int chargingStationId)
        {
            var chargingStation = await _unitOfWork.ChargingStationRepository.GetAsync(chargingStationId);
            return _mapper.Map<ChargingStation, ChargingStationDto>(chargingStation);
        }

        public async Task<List<ChargerDto>> GetChargersAsync(int chargingStationId)
        {
            var chargingStations = await GetAsync(chargingStationId);

            if (chargingStations == null)
                return null;

            var chargers = await _unitOfWork.ChargerRepository.GetAsync();

            if (chargers == null)
                return new List<ChargerDto>();

            chargers = chargers.Where(x => x.ChargingStationId == chargingStationId).ToList();
            return _mapper.Map<List<Charger>, List<ChargerDto>>(chargers);
        }
    }
}
