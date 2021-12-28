using AutoMapper;
using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.Shared;
using System.Threading.Tasks;

namespace ChargingStations.Application.Chargers
{
    public class ChargerService : IChargerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        
        public ChargerService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ChargerDto> GetAsync(int chargerId)
        {
            var charger = await _unitOfWork.ChargerRepository.GetAsync(chargerId);
            var chargingStation = await _unitOfWork.ChargingStationRepository.GetAsync(charger.ChargingStationId);

            if (charger.Id == 0 || chargingStation == null)
                return null;

            var chargerDto = _mapper.Map<Charger,ChargerDto>(charger);
            chargerDto.ChargingStationName = chargingStation.Name;
            chargerDto.Address = chargingStation.Address;

            var chargerModel = await _unitOfWork.ChargerModelRepository.GetAsync(charger.ChargerModelId);
            chargerDto.ModelName = chargerModel.Name;
            chargerDto.Manufacturer = chargerModel.Manufacturer;
            return chargerDto;
        }
    }
}
