using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models
{
    public class Coordinate
    {
        [Key]
        [JsonIgnore]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public int X { get; set; }
        public int Y { get; set; }
        public Kleur Kleur { get; set; }

        [JsonIgnore]
        public int Order { get; set; }

        [JsonIgnore]
        public string SpelId { get; set; }
        [JsonIgnore]
        public virtual Spel Spel { get; set; }
    }
}
