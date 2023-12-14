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
    internal class GroupeRepository : IGroupeRepository
    {
        private readonly IMongoCollection<Groupe> _collection;

        public GroupeRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Groupe>("groupes");
        }
        //Récupère les groupes pour les ajouter au calendrier
        public List<Groupe> GetGroupeCollection()
        {
            return _collection.Find(Builders<Groupe>.Filter.Empty).ToList();
        }
    }
}
