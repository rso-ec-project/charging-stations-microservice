using System;

namespace ChargingStations.Application.ReservationsMicroService.ReservationSlots
{
    public class ReservationSlotDto
    {
        public int ChargerId { get; set; }
        public string ChargerName { get; set; }
        public int Duration { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
