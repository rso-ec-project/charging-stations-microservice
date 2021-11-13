using AutoMapper;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.Shared;
using System.Collections.Generic;
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
    }
}
