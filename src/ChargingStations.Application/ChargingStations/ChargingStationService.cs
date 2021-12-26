using AutoMapper;
using ChargingStations.Application.Chargers;
using ChargingStations.Application.CommentsMicroservice.Ratings;
using ChargingStations.Application.ReservationsMicroService.ReservationSlots;
using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.Shared;
using System;
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
        private readonly IReservationSlotService _reservationSlotService;

        public ChargingStationService(IMapper mapper, IUnitOfWork unitOfWork, IRatingService ratingService, IReservationSlotService reservationSlotService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _ratingService = ratingService;
            _reservationSlotService = reservationSlotService;
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

            var chargers = await _unitOfWork.ChargerRepository.GetAsync();

            var reservationSlotDtos = new List<ReservationSlotDto>();
            chargers ??= new List<Charger>();

            chargers = chargers.Where(x => x.ChargingStationId == chargingStationId).ToList();

            foreach (var chargerDto in chargers)
            {
                var reservationSlots = await _reservationSlotService.GetAsync(chargerDto.Id,
                    new DateTime(2021, 12, 19, 9, 0, 0), new DateTime(2021, 12, 19, 16, 0, 0));

                if (reservationSlots == null)
                {
                    reservationSlotDtos.Add(null);
                }
                else
                {
                    reservationSlotDtos.AddRange(reservationSlots);
                }
            }

            if (reservationSlotDtos.All(x => x == null))
            {
                chargingStationDto.ReservationSlots = null;
            }
            else
            {
                chargingStationDto.ReservationSlots = reservationSlotDtos.Where(x => x != null).OrderBy(x => x.From).ToList();
                foreach (var reservationSlotDto in chargingStationDto.ReservationSlots)
                {
                    var charger = chargers.FirstOrDefault(x => x.Id == reservationSlotDto.ChargerId);
                    reservationSlotDto.ChargerName = charger != null ? charger.Name : "";
                }
            }

            return chargingStationDto;
        }

        public async Task<List<ChargerDto>> GetChargersAsync(int chargingStationId)
        {
            var chargingStation = await _unitOfWork.ChargingStationRepository.GetAsync(chargingStationId);

            if (chargingStation == null)
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
