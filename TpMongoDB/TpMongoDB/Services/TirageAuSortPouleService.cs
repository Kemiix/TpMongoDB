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
        // Méthode principale pour effectuer le tirage au sort
        public void EffectuerTirageAuSort()
        {
            _groupesCollection.DeleteMany(Builders<Groupe>.Filter.Empty);

            // Récupération de la liste de toutes les équipes
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

                // Vérifier si le groupe est 'A' car pour Allemagne placer en A1
                if (groupeLettre == 'A')
                {
                    // Vérifier si l'Allemagne est dans le chapeau 1
                    if (chapeaux.ContainsKey(1) && chapeaux[1].Any(e => e.CountryName == "Germany"))
                    {
                        var equipeAllemagneChapeau1 = chapeaux[1].First(e => e.CountryName == "Germany");
                        groupe.EquipePosition1 = equipeAllemagneChapeau1;
                        chapeaux[1].Remove(equipeAllemagneChapeau1);

                        // Ajouter une équipe de chaque chapeau restant
                        for (int chapeauIndex = 2; chapeauIndex <= 4; chapeauIndex++)
                        {
                            if (chapeaux.ContainsKey(chapeauIndex) && chapeaux[chapeauIndex].Any())
                            {
                                var equipeIndex = random.Next(chapeaux[chapeauIndex].Count);
                                var equipe = chapeaux[chapeauIndex][equipeIndex];

                                switch (chapeauIndex)
                                {
                                    case 2:
                                        groupe.EquipePosition2 = equipe;
                                        break;
                                    case 3:
                                        groupe.EquipePosition3 = equipe;
                                        break;
                                    case 4:
                                        groupe.EquipePosition4 = equipe;
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
                                groupe.EquipePosition1 = equipe;
                                break;
                            case 2:
                                groupe.EquipePosition2 = equipe;
                                break;
                            case 3:
                                groupe.EquipePosition3 = equipe;
                                break;
                            case 4:
                                groupe.EquipePosition4 = equipe;
                                break;
                        }

                        // Si l'équipe vient des barrages, utilisez sa position spécifique
                        if (!string.IsNullOrEmpty(equipe.Playoff))
                        {
                            AssignerPositionBarrages(groupe, equipe, groupeLettre);
                        }

                        chapeaux[chapeauKey].RemoveAt(equipeIndex);
                    }

                    // Ordre des équipes dans le groupe
                    groupe.MelangerEquipes(random);
                }

                groupes.Add(groupe);
            }

            _groupesCollection.InsertMany(groupes);
        }

        private void AssignerPositionBarrages(Groupe groupe, Equipe equipe, char groupeLettre)
        {
            var position = equipe.Playoff;

            // Position spécifique pour les équipes issues des barrages 
            switch (groupeLettre)
            {
                case 'B':
                    groupe.EquipePosition2 = equipe;
                    break;
                case 'C':
                    groupe.EquipePosition3 = equipe;
                    break;
                case 'D':
                    groupe.EquipePosition4 = equipe;
                    break;
                case 'E':
                    groupe.EquipePosition4 = equipe; 
                    break;
                case 'F':
                    groupe.EquipePosition4 = equipe;
                    break;
            }
        }
    }

}