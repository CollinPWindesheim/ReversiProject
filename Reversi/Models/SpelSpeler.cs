using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models
{
    public class SpelSpeler
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string SpelId { get; set; }
        public virtual Spel Spel { get; set; }

        public string SpelerId { get; set; }
        public virtual Speler Speler { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public Kleur Kleur { get; set; }
    }
}
