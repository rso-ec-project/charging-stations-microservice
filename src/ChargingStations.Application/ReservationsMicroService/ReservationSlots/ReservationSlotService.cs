using ChargingStations.Application.Shared;
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

        public ReservationSlotService(ReservationsMicroServiceClient reservationsMicroServiceClient)
        {
            _reservationsMicroServiceClient = reservationsMicroServiceClient;
        }

        public async Task<List<ReservationSlotDto>> GetAsync(int chargerId, DateTime from, DateTime to)
        {
            try
            {
                var responseMessage = _reservationsMicroServiceClient.Client.GetAsync($"ReservationSlots?chargerId={chargerId}&from={@from:yyyy-MM-ddTHH:mm:ss}&to={@to:yyyy-MM-ddTHH:mm:ss}").Result;

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(responseMessage.StatusCode.ToString());
                }

                await using var responseStream = await responseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<List<ReservationSlotDto>>(responseStream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
