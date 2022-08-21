using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingItem.Models;

    public class Bookings {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string OriginAirport { get; set; } = null!;
        public string DestinationAirport { get; set; } = null!;

    }
