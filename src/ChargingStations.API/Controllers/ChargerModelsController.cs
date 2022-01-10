using ChargingStations.Application.ChargerModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class ChargerModelsController : ControllerBase
    {
        private readonly IChargerModelService _chargerModelService;

        public ChargerModelsController(IChargerModelService chargerModelService)
        {
            _chargerModelService = chargerModelService;
        }

        /// <summary>
        /// Get a list of all charger models.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ChargerModelDto>>> Get()
        {
            return await _chargerModelService.GetAsync();
        }
    }
}
