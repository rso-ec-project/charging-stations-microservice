using ChargingStations.Application.Chargers;
using ChargingStations.Application.ChargingStations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ChargingStations.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class ChargingStationsController : ControllerBase
    {
        private readonly IChargingStationService _chargingStationService;

        public ChargingStationsController(IChargingStationService chargingStationService)
        {
            _chargingStationService = chargingStationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ChargingStationDto>>> Get([FromQuery, Required] double lat, [FromQuery, Required] double lng)
        {
            return await _chargingStationService.GetAsync(lat, lng);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ChargingStationDto>> Get(int id, [FromQuery] double? lat = null, [FromQuery] double? lng = null)
        {
            var chargingStation = await _chargingStationService.GetAsync(id, lat, lng);

            if (chargingStation == null)
                return NotFound();

            return chargingStation;
        }

        [HttpGet("{id}/Chargers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ChargerDto>>> GetChargers(int id)
        {
            var chargers = await _chargingStationService.GetChargersAsync(id);

            if (chargers == null)
                return NotFound();

            return chargers;
        }
    }
}
