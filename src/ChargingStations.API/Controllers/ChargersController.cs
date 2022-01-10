using ChargingStations.Application.Chargers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChargingStations.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class ChargersController : ControllerBase
    {
        private readonly IChargerService _chargerService;

        public ChargersController(IChargerService chargerService)
        {
            _chargerService = chargerService;
        }

        /// <summary>
        /// Get a single charger by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ChargerDto>> Get(int id)
        {
            var charger = await _chargerService.GetAsync(id);

            if (charger == null)
                return NotFound();

            return charger;
        }
    }
}
