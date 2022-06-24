using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models
{
    public enum MessageType { Message, Connect, Disconnect }
    public class Message
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public MessageType Type { get; set; }

        public string Content { get; set; }

        public DateTime DateCreate { get; set; } = DateTime.Now;

        public Kleur Color => SpelSpeler?.Kleur ?? Kleur.Geen;

        public string SpelId => SpelSpeler?.SpelId;

        [JsonIgnore]
        public string SpelSpelerId { get; set; }
        [JsonIgnore]
        public virtual SpelSpeler SpelSpeler { get; set; }
    }
}
