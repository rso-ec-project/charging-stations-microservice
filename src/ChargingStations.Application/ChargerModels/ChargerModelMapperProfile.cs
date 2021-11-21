using AutoMapper;
using ChargingStations.Domain.ChargerModelAggregate;

namespace ChargingStations.Application.ChargerModels
{
    public class ChargerModelMapperProfile : Profile
    {
        public ChargerModelMapperProfile()
        {
            CreateMap<ChargerModel, ChargerModelDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Manufacturer, opts => opts.MapFrom(src => src.Manufacturer));
        }
    }
}
