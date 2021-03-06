using ChargingStations.Application.ChargingStations;
using ChargingStations.Application.Tenants;
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
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        /// <summary>
        /// Get a list of all tenants.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<TenantDto>>> Get()
        {
            return await _tenantService.GetAsync();
        }

        /// <summary>
        /// Get a single tenant by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TenantDto>> Get(int id)
        {
            var tenant = await _tenantService.GetAsync(id);

            if (tenant == null)
                return NotFound();

            return tenant;
        }

        /// <summary>
        /// Get all charging stations of a single tenant.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/ChargingStations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ChargingStationDto>>> GetChargingStations(int id)
        {
            var chargingStations = await _tenantService.GetChargingStationsAsync(id);

            if (chargingStations == null)
                return NotFound();

            return chargingStations;
        }
    }
}
