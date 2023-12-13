using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpMongoDB.Models;
using TpMongoDB.Repositories;

namespace TpMongoDB.Services
{
    internal class TirageAuSortPouleService
    {
        private readonly EquipeRepository _equipeRepository;
        private readonly IMongoCollection<Groupe> _groupesCollection;

        public TirageAuSortPouleService(EquipeRepository equipeRepository, IMongoDatabase database)
        {
            _equipeRepository = equipeRepository;
            _groupesCollection = database.GetCollection<Groupe>("groupes");
        }

        public void EffectuerTirageAuSort()
        {
            _groupesCollection.DeleteMany(Builders<Groupe>.Filter.Empty);

            var equipes = _equipeRepository.GetAllEquipes();

            var chapeaux = equipes.GroupBy(e => e.Hat).ToDictionary(g => g.Key, g => g.ToList());

            // Ajout des équipes des barrages aux chapeaux
            var equipesBarrages = equipes.Where(e => !string.IsNullOrEmpty(e.Playoff)).ToList();
            foreach (var equipeBarrage in equipesBarrages)
            {
                chapeaux[equipeBarrage.Hat].Add(equipeBarrage);
            }

            var groupes = new List<Groupe>();
            var random = new Random();

            foreach (char groupeLettre in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
            {
                var groupe = new Groupe { Nom = groupeLettre.ToString() };

                foreach (var chapeauKey in chapeaux.Keys)
                {
                    var equipeIndex = random.Next(chapeaux[chapeauKey].Count);
                    var equipe = chapeaux[chapeauKey][equipeIndex];

                    switch (chapeauKey)
                    {
                        case 1:
                            groupe.EquipeChapeau1 = equipe;
                            break;
                        case 2:
                            groupe.EquipeChapeau2 = equipe;
                            break;
                        case 3:
                            groupe.EquipeChapeau3 = equipe;
                            break;
                        case 4:
                            groupe.EquipeChapeau4 = equipe;
                            break;
                    }

                    // Si l'équipe vient des barrages, utilisez sa position spécifique
                    if (!string.IsNullOrEmpty(equipe.Playoff))
                    {
                        switch (groupeLettre)
                        {
                            case 'B':
                                groupe.PositionB = equipe.Playoff;
                                break;
                            case 'C':
                                groupe.PositionC = equipe.Playoff;
                                break;
                            case 'D':
                                groupe.PositionD = equipe.Playoff;
                                break;
                            case 'E':
                                groupe.PositionE = equipe.Playoff;
                                break;
                            case 'F':
                                groupe.PositionF = equipe.Playoff;
                                break;
                        }
                    }

                    chapeaux[chapeauKey].RemoveAt(equipeIndex);
                }

                groupes.Add(groupe);
            }

            _groupesCollection.InsertMany(groupes);
        }
    }
}
