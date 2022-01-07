using ChargingStations.Application.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChargingStations.Application.ReservationsMicroService.ReservationSlots
{
    public class ReservationSlotService : IReservationSlotService
    {
        private readonly ReservationsMicroServiceClient _reservationsMicroServiceClient;
        private readonly ILogger<ReservationSlotService> _logger;

        public ReservationSlotService(ReservationsMicroServiceClient reservationsMicroServiceClient, ILogger<ReservationSlotService> logger)
        {
            _reservationsMicroServiceClient = reservationsMicroServiceClient;
            _logger = logger;
        }

        public async Task<List<ReservationSlotDto>> GetAsync(int chargerId, DateTime from, DateTime to)
        {
            var endpoint = $"endpoint ReservationSlots?chargerId={chargerId}&from={@from:yyyy-MM-ddTHH:mm:ss}&to={@to:yyyy-MM-ddTHH:mm:ss}";

            try
            {
                _logger.LogInformation($"Entered Reservations MS {endpoint}");

                var responseMessage = _reservationsMicroServiceClient.Client.GetAsync($"ReservationSlots?chargerId={chargerId}&from={@from:yyyy-MM-ddTHH:mm:ss}&to={@to:yyyy-MM-ddTHH:mm:ss}").Result;

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(responseMessage.StatusCode.ToString());
                }

                await using var responseStream = await responseMessage.Content.ReadAsStreamAsync();

                var reservationSlotDtos = await JsonSerializer.DeserializeAsync<List<ReservationSlotDto>>(responseStream);
                _logger.LogInformation($"Exited Comments MS {endpoint} with: 200 OK");

                return reservationSlotDtos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exited Reservations MS {endpoint} with: Exception {e}");
                return null;
            }
        }
    }
}
