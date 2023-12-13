using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpMongoDB.Models
{
    internal class Groupe
    {
        public string Nom { get; set; }
        public Equipe EquipeChapeau1 { get; set; }
        public Equipe EquipeChapeau2 { get; set; }
        public Equipe EquipeChapeau3 { get; set; }
        public Equipe EquipeChapeau4 { get; set; }

        // Positions spécifiques pour les chapeaux B à F
        public string PositionB { get; set; }
        public string PositionC { get; set; }
        public string PositionD { get; set; }
        public string PositionE { get; set; }
        public string PositionF { get; set; }
    }
}
