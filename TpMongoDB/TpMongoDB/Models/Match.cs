using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using TpMongoDB.Interfaces;

namespace TpMongoDB.Models
{
    internal class Match
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        // Nom du groupe comme GroupeId
        public string GroupeId { get; set; }

        // Nom de l'équipe comme Equipe1
        public string Equipe1 { get; set; }

        // Nom de l'équipe comme Equipe2
        public string Equipe2 { get; set; }

        public Resultat Resultat { get; set; }
    }
}