using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models
{
    public class Speler : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }

        public DateTime DateCreate { get; set; } = DateTime.Now;

        public int Won => SpelSpelers.Count(sss => sss.Spel.SpelState == SpelState.Finished && sss.Spel.OverwegendeKleur() == sss.Kleur);
        public int Lost => SpelSpelers.Count(sss => sss.Spel.SpelState == SpelState.Finished && sss.Spel.OverwegendeKleur() == Spel.ReverseKleur(sss.Kleur));
        public int Draw => SpelSpelers.Count(sss => sss.Spel.SpelState == SpelState.Finished && sss.Spel.OverwegendeKleur() == Kleur.Geen);
        public int Forfeit => SpelSpelers.Count(sss => (sss.Spel.SpelState == SpelState.ForfeitWit && sss.Kleur == Kleur.Wit) || (sss.Spel.SpelState == SpelState.ForfeitZwart && sss.Kleur == Kleur.Zwart));
        public int Sum => SpelSpelers.Count(sss => sss.Spel.SpelState == SpelState.Finished || (sss.Spel.SpelState == SpelState.ForfeitWit && sss.Kleur == Kleur.Wit) || (sss.Spel.SpelState == SpelState.ForfeitZwart && sss.Kleur == Kleur.Zwart));

        public virtual ICollection<SpelSpeler> SpelSpelers { get; set; }
    }
}
