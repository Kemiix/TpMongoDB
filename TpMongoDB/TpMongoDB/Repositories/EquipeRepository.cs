using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpMongoDB.Models;

namespace TpMongoDB.Repositories
{
    internal class EquipeRepository
    {
        public readonly IMongoCollection<Equipe> _collection;

        // Le constructeur
        public EquipeRepository(IMongoDatabase database)
        {

            _collection = database.GetCollection<Equipe>("equipes");
        }

        // Méthode pour récupérer toutes les équipes depuis la collection
        public List<Equipe> GetAllEquipes()
        {
            return _collection.Find(Builders<Equipe>.Filter.Empty).ToList();
        }




    }
}
