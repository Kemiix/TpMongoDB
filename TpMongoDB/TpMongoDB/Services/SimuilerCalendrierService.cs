using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using TpMongoDB.Interfaces;
using TpMongoDB.Models;

namespace TpMongoDB.Services
{
    public class SimuilerCalendrierService
    {
        private readonly IGroupeRepository _groupesCollection;
        private readonly IMongoCollection<Match> _matchsCollection;

        public SimuilerCalendrierService(IGroupeRepository groupeRepository, IMongoDatabase database)
        {
            _groupesCollection = groupeRepository;
            _matchsCollection = database.GetCollection<Match>("Match");
        }

        public void GenererCalendrierMatches()
        {
            // Supprimer les matchs existants (s'il y en a) avant de les générer à nouveau
            _matchsCollection.DeleteMany(Builders<Match>.Filter.Empty);

     
            // Récupération de la liste de tous les groupes
            var groupes = _groupesCollection.GetGroupeCollection();
            foreach (var groupe in groupes)
            {
                var equipesDansLeGroupe = new List<Equipe>
                {
                    groupe.EquipePosition1,
                    groupe.EquipePosition2,
                    groupe.EquipePosition3,
                    groupe.EquipePosition4
                };

                // Utiliser le nom du groupe comme base pour créer un identifiant unique
                var groupeId = groupe.Id.ToString(); // Convertir l'ObjectId en chaîne

                for (int i = 0; i < equipesDansLeGroupe.Count - 1; i++)
                {
                    for (int j = i + 1; j < equipesDansLeGroupe.Count; j++)
                    {
                        var match = new Match
                        {
                            GroupeId = groupe.Nom, // Utiliser le nom du groupe
                            Equipe1 = equipesDansLeGroupe[i].CountryName, // Utiliser le nom de l'équipe
                            Equipe2 = equipesDansLeGroupe[j].CountryName, // Utiliser le nom de l'équipe
                            Resultat = new Resultat() // Initialiser le résultat du match
                        };

                        _matchsCollection.InsertOne(match);
                    }
                }
            }
        }
    }
}