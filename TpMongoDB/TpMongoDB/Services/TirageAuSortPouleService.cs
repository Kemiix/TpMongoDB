using MongoDB.Driver;
using System;
using System.Collections.Generic;
using TpMongoDB.Interfaces;
using TpMongoDB.Models;
using TpMongoDB.Repositories;
using System.Linq;

namespace TpMongoDB.Services
{
    public class TirageAuSortPouleService
    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly IMongoCollection<Groupe> _groupesCollection;

        public TirageAuSortPouleService(IEquipeRepository equipeRepository, IMongoDatabase database)
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

            // Boucle principale
            foreach (char groupeLettre in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
            {
                var groupe = new Groupe { Nom = groupeLettre.ToString() };

                // Vérifier si le groupe est 'A'
                if (groupeLettre == 'A')
                {
                    // Vérifier si l'Allemagne est dans le chapeau 1
                    if (chapeaux.ContainsKey(1) && chapeaux[1].Any(e => e.CountryName == "Germany"))
                    {
                        var equipeAllemagneChapeau1 = chapeaux[1].First(e => e.CountryName == "Germany");
                        groupe.EquipeChapeau1 = equipeAllemagneChapeau1;
                        chapeaux[1].Remove(equipeAllemagneChapeau1);

                        // Ajouter trois équipes supplémentaires si le chapeau a suffisamment d'équipes
                        for (int i = 0; i < 3 && chapeaux.Count > 1; i++)
                        {
                            var chapeauIndex = random.Next(2, 5); // Choix aléatoire parmi les chapeaux 2, 3 et 4

                            // Vérifier si le chapeau de l'indice sélectionné contient des équipes
                            if (chapeaux.ContainsKey(chapeauIndex) && chapeaux[chapeauIndex].Any())
                            {
                                var equipeIndex = random.Next(chapeaux[chapeauIndex].Count);
                                var equipe = chapeaux[chapeauIndex][equipeIndex];

                                switch (i)
                                {
                                    case 0:
                                        groupe.EquipeChapeau2 = equipe;
                                        break;
                                    case 1:
                                        groupe.EquipeChapeau3 = equipe;
                                        break;
                                    case 2:
                                        groupe.EquipeChapeau4 = equipe;
                                        break;
                                }

                                chapeaux[chapeauIndex].RemoveAt(equipeIndex);
                            }
                        }
                    }
                }
                else
                {
                    // Tirage au sort pour chaque chapeau
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
                            AssignerPositionBarrages(groupe, equipe, groupeLettre);
                        }

                        chapeaux[chapeauKey].RemoveAt(equipeIndex);
                    }

                    // Mélanger l'ordre des équipes dans le groupe
                    groupe.MelangerEquipes(random);
                }

                groupes.Add(groupe);
            }

            _groupesCollection.InsertMany(groupes);
        }

        private void AssignerPositionBarrages(Groupe groupe, Equipe equipe, char groupeLettre)
        {
            var position = equipe.Playoff;

            // Assigner la position spécifique pour les équipes issues des barrages
            switch (groupeLettre)
            {
                case 'B':
                    groupe.EquipeChapeau2 = equipe;
                    break;
                case 'C':
                    groupe.EquipeChapeau3 = equipe;
                    break;
                case 'D':
                    groupe.EquipeChapeau4 = equipe;
                    break;
                case 'E':
                    groupe.EquipeChapeau4 = equipe; // Vous pouvez ajuster ici pour les groupes E et F
                    break;
                case 'F':
                    groupe.EquipeChapeau4 = equipe; // Vous pouvez ajuster ici pour le groupe F
                    break;
            }
        }
    }

}