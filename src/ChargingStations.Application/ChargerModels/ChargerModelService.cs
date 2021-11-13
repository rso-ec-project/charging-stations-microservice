using AutoMapper;
using ChargingStations.Domain.ChargerModelAggregate;
using ChargingStations.Domain.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Application.ChargerModels
{
    public class ChargerModelService : IChargerModelService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ChargerModelService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ChargerModelDto>> GetAsync()
        {
            var chargerModels = await _unitOfWork.ChargerModelRepository.GetAsync();
            return _mapper.Map<List<ChargerModel>, List<ChargerModelDto>>(chargerModels);
        }
    }
}
