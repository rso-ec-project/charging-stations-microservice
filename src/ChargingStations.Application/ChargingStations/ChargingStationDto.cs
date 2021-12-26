using ChargingStations.Application.CommentsMicroservice.Ratings;
using ChargingStations.Application.ReservationsMicroService.ReservationSlots;
using System.Collections.Generic;

namespace ChargingStations.Application.ChargingStations
{
    public class ChargingStationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int TenantId { get; set; }
        public RatingDto RatingDetails { get; set; }
        public IEnumerable<ReservationSlotDto> ReservationSlots { get; set; }
    }
}
