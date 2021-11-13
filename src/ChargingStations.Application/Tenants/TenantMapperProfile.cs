using AutoMapper;
using ChargingStations.Domain.TenantAggregate;

namespace ChargingStations.Application.Tenants
{
    public class TenantMapperProfile : Profile
    {
        public TenantMapperProfile()
        {
            CreateMap<Tenant, TenantDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.TenantId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Address))
                ;
        }
    }
}
