﻿using ChargingStations.Application.Chargers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Application.ChargingStations
{
    public interface IChargingStationService
    {
        Task<List<ChargingStationDto>> GetAsync();
        Task<ChargingStationDto> GetAsync(int chargingStationId);
        Task<List<ChargerDto>> GetChargersAsync(int chargingStationId);
    }
}