using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpMongoDB.Models
{
    public class Groupe
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Nom { get; set; }
        public Equipe EquipePosition1 { get; set; }
        public Equipe EquipePosition2 { get; set; }
        public Equipe EquipePosition3 { get; set; }
        public Equipe EquipePosition4 { get; set; }

        // Méthode pour mélanger l'ordre des équipes dans le groupe
        public void MelangerEquipes(Random random)
        {
            var proprieteEquipes = GetType().GetProperties().Where(p => p.Name.StartsWith("EquipePosition")).ToList();
            var equipes = proprieteEquipes.Select(p => p.GetValue(this) as Equipe).ToList();

            // Mélanger l'ordre des équipes dans le groupe
            for (int i = equipes.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                var temp = equipes[i];
                equipes[i] = equipes[j];
                equipes[j] = temp;
            }

            // Réaffecter les équipes mises à jour au groupe
            for (int i = 0; i < proprieteEquipes.Count; i++)
            {
                proprieteEquipes[i].SetValue(this, equipes[i]);
            }
        }
    }
}
