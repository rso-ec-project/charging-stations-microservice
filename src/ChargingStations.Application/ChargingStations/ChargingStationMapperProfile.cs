using AutoMapper;
using ChargingStations.Application.ReservationsMicroService.ReservationSlots;
using ChargingStations.Domain.ChargingStationAggregate;
using System.Collections.Generic;

namespace ChargingStations.Application.ChargingStations
{
    public class ChargingStationMapperProfile : Profile
    {
        public ChargingStationMapperProfile()
        {
            CreateMap<ChargingStation, ChargingStationDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Address))
                .ForMember(dest => dest.Latitude, opts => opts.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opts => opts.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.TenantId, opts => opts.MapFrom(src => src.TenantId))
                .ForMember(dest => dest.ReservationSlots, opts => opts.MapFrom(src => new List<ReservationSlotDto>()))
                ;
        }
    }
}
