using ChargingStations.Application.Chargers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChargingStations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargersController : ControllerBase
    {
        private readonly IChargerService _chargerService;

        public ChargersController(IChargerService chargerService)
        {
            _chargerService = chargerService;
        }

        [HttpGet("id")]
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
