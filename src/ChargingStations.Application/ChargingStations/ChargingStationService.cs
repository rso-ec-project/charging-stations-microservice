using AutoMapper;
using ChargingStations.Application.Chargers;
using ChargingStations.Application.CommentsMicroservice.Ratings;
using ChargingStations.Application.Distances;
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
        private readonly IDistanceService _distanceService;

        public ChargingStationService(IMapper mapper, IUnitOfWork unitOfWork, IRatingService ratingService, 
            IReservationSlotService reservationSlotService, IDistanceService distanceService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _ratingService = ratingService;
            _reservationSlotService = reservationSlotService;
            _distanceService = distanceService;
        }

        public async Task<List<ChargingStationDto>> GetAsync(double latitude, double longitude)
        {
            var chargingStations = await _unitOfWork.ChargingStationRepository.GetAsync();
            var chargingStationDtos = _mapper.Map<List<ChargingStation>, List<ChargingStationDto>>(chargingStations);

            foreach (var chargingStationDto in chargingStationDtos)
            {
                var distance = await _distanceService.GetAsync(latitude, longitude, chargingStationDto.Latitude, chargingStationDto.Longitude);

                if (distance != null)
                {
                    chargingStationDto.DistanceFromLocation = distance.Distance;
                }
            }

            return chargingStationDtos;
        }

        public async Task<ChargingStationDto> GetAsync(int chargingStationId, double? latitude, double? longitude)
        {
            var chargingStation = await _unitOfWork.ChargingStationRepository.GetAsync(chargingStationId);
            var chargingStationDto = _mapper.Map<ChargingStation, ChargingStationDto>(chargingStation);
            if (chargingStation == null)
                return null;

            chargingStationDto.RatingDetails = await _ratingService.GetAsync(chargingStationId);

            var chargers = await _unitOfWork.ChargerRepository.GetAsync();

            var reservationSlotDtos = new List<ReservationSlotDto>();
            chargers ??= new List<Charger>();

            chargers = chargers.Where(x => x.ChargingStationId == chargingStationId).ToList();

            var startTime = DateTime.Now;
            var endTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, 23, 59, 0).AddDays(1);

            foreach (var chargerDto in chargers)
            {
                var reservationSlots = await _reservationSlotService.GetAsync(chargerDto.Id, startTime, endTime);

                if (reservationSlots == null)
                {
                    reservationSlotDtos.Add(null);
                }
                else
                {
                    reservationSlotDtos.AddRange(reservationSlots);
                }
            }

            if (!reservationSlotDtos.Any())
            {
                chargingStationDto.ReservationSlots = new List<ReservationSlotDto>();
            }
            else if (reservationSlotDtos.All(x => x == null))
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

            //if (latitude != null && longitude != null)
            //{
            //    var distance = await _distanceService.GetAsync((double) latitude, (double) longitude, chargingStationDto.Latitude, chargingStationDto.Longitude);

            //    if (distance != null)
            //    {
            //        chargingStationDto.DistanceFromLocation = distance.Distance;
            //    }
            //}

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
                chargerDto.ChargingStationName = chargingStation.Name;
                chargerDto.Address = chargingStation.Address;
                chargerDtos.Add(chargerDto);
            }

            return chargerDtos;
        }
    }
}
