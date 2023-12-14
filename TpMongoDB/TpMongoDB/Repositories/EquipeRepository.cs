using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpMongoDB.Interfaces;
using TpMongoDB.Models;

namespace TpMongoDB.Repositories
{
    internal class EquipeRepository : IEquipeRepository
    {
        private readonly IMongoCollection<Equipe> _collection;

        public EquipeRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Equipe>("equipes");
        }
        //Récupère les équipes pour les ajouter à la liste d'équipe
        public List<Equipe> GetAllEquipes()
        {
            return _collection.Find(Builders<Equipe>.Filter.Empty).ToList();
        }
    }
}
