using AutoMapper;
using ChargingStations.Domain.ChargerAggregate;

namespace ChargingStations.Application.Chargers
{
    public class ChargerMapperProfile : Profile
    {
        public ChargerMapperProfile()
        {
            CreateMap<Charger, ChargerDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.ChargerId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.ChargingFeePerKwh, opts => opts.MapFrom(src => src.ChargingFeePerKwh))
                ;
        }
    }
}
