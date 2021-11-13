using ChargingStations.Application.ChargerModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargerModelsController : ControllerBase
    {
        private readonly IChargerModelService _chargerModelService;

        public ChargerModelsController(IChargerModelService chargerModelService)
        {
            _chargerModelService = chargerModelService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ChargerModelDto>>> Get()
        {
            return await _chargerModelService.GetAsync();
        }
    }
}
