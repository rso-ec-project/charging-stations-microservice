using AutoMapper;
using ChargingStations.Application.Chargers;
using ChargingStations.Application.CommentsMicroservice.Ratings;
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
        private readonly IRatingService _ratingService;

        public ChargingStationService(IMapper mapper, IUnitOfWork unitOfWork, IRatingService ratingService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _ratingService = ratingService;
        }

        public async Task<List<ChargingStationDto>> GetAsync()
        {
            var chargingStations = await _unitOfWork.ChargingStationRepository.GetAsync();
            return _mapper.Map<List<ChargingStation>, List<ChargingStationDto>>(chargingStations);
        }

        public async Task<ChargingStationDto> GetAsync(int chargingStationId)
        {
            var chargingStation = await _unitOfWork.ChargingStationRepository.GetAsync(chargingStationId);
            var chargingStationDto = _mapper.Map<ChargingStation, ChargingStationDto>(chargingStation);
            if (chargingStation != null)
                chargingStationDto.RatingDetails = await _ratingService.GetAsync(chargingStationId);
            return chargingStationDto;
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

            var chargerDtos = new List<ChargerDto>();

            foreach (var charger in chargers)
            {
                var chargerDto = _mapper.Map<Charger, ChargerDto>(charger);
                var chargerModel = await _unitOfWork.ChargerModelRepository.GetAsync(charger.ChargerModelId);

                chargerDto.Manufacturer = chargerModel.Manufacturer;
                chargerDto.ModelName = chargerModel.Name;
                chargerDtos.Add(chargerDto);
            }

            return chargerDtos;
        }
    }
}
