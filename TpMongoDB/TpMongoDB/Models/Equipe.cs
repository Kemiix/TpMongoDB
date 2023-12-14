using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TpMongoDB.Models
{
    public class Equipe
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Country name")]
        public string CountryName { get; set; }

        [BsonElement("FIFA ranking")]
        public int FifaRanking { get; set; }

        public string Code { get; set; }

        public int Hat { get; set; }
        public string? Playoff { get; set; }
    }
}