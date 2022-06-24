using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models
{
    public enum SpelState { Waiting, Ongoing, Finished, ForfeitZwart, ForfeitWit }

    public class Spel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public virtual ICollection<SpelSpeler> SpelSpelers { get; set; }

        public string Omschrijving { get; set; }

        public DateTime DateCreate { get; set; } = DateTime.Now;

        public SpelState SpelState { get; set; } = SpelState.Waiting;

        public Kleur AandeBeurt
        {
            get => _aanDeBeurt;
            set
            {
                if (value == Kleur.Zwart || value == Kleur.Wit)
                {
                    _aanDeBeurt = value;
                }
            }
        }
        private Kleur _aanDeBeurt = Kleur.Zwart;

        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        public Kleur[,] Bord { get; set; }

        [Required]
        public string SerializedBord { get => JsonConvert.SerializeObject(Bord); set => Bord = JsonConvert.DeserializeObject<Kleur[,]>(value); }

        public int BordGrootte 
        { 
            get => _bordGrootte; 
            set
            {
                if (value < 6)
                {
                    _bordGrootte = 6;
                    return;
                }
                if (value > 24)
                {
                    _bordGrootte = 24;
                    return;
                }

                _bordGrootte = value + value % 2;
            } 
        }
        private int _bordGrootte = StandaardBordGrootte;
        public const int StandaardBordGrootte = 8;

        public virtual ICollection<Coordinate> Coordinates { get; set; }

        public void Start(bool unitTesting = false)
        {
            Bord = new Kleur[BordGrootte, BordGrootte];
            int start = (int)Math.Ceiling(BordGrootte / 2d) - 1;
            Bord[start, start] = Kleur.Wit;
            Bord[start + 1, start] = Kleur.Zwart;
            Bord[start, start + 1] = Kleur.Zwart;
            Bord[start + 1, start + 1] = Kleur.Wit;

            if (unitTesting)
            {
                Coordinates = new List<Coordinate>();
            }
        }

        public List<Coordinate> GetBord()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (int y = 0; y < BordGrootte; y++)
            {
                for (int x = 0; x < BordGrootte; x++)
                {
                    if (Bord[y,x] != Kleur.Geen)
                    {
                        coordinates.Add(new Coordinate()
                        {
                            X = x,
                            Y = y,
                            Kleur = Bord[y,x]
                        });
                    }
                }
            }
            return coordinates;
        }
        public List<Coordinate> GetMoves(Kleur kleur)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            if (kleur == AandeBeurt)
            {
                for (int y = 0; y < BordGrootte; y++)
                {
                    for (int x = 0; x < BordGrootte; x++)
                    {
                        if (Bord[y, x] == Kleur.Geen && ZetMogelijk(y, x, kleur))
                        {
                            coordinates.Add(new Coordinate()
                            {
                                X = x,
                                Y = y,
                                Kleur = Kleur.Geen
                            });
                        }
                    }
                }
            }
            return coordinates;
        }

        public bool Afgelopen()
        {
            int count = 0;

            bool possibleMove = false;
            for (int y = 0; y < BordGrootte && !possibleMove; y++)
            {
                for (int x = 0; x < BordGrootte && !possibleMove; x++)
                {
                    if (Bord[y, x] == Kleur.Geen)
                    {
                        possibleMove = ZetMogelijk(y, x, Kleur.Wit) || ZetMogelijk(y, x, Kleur.Zwart);
                        count++;
                    }
                }
            }

            bool finished = !possibleMove || count < 1;
            if (finished)
            {
                SpelState = SpelState.Finished;
            }

            return finished;
        }

        public bool DoeZet(int rijZet, int kolomZet)
        {
            if (!ZetMogelijk(rijZet, kolomZet))
            {
                return false;
            }

            Bord[rijZet, kolomZet] = AandeBeurt;
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (ControleerSequence(rijZet, kolomZet, x, y, AandeBeurt))
                    {
                        UpdateSequence(rijZet, kolomZet, x, y);
                    }
                }
            }

            int order = Coordinates.Count() > 0 ? Coordinates.Select(c => c.Order).Max() : 0;
            Coordinates?.Add(new Coordinate()
            {
                X = kolomZet,
                Y = rijZet,
                Kleur = AandeBeurt,
                SpelId = Id,
                Order = order + 1
            });

            AandeBeurt = ReverseKleur(AandeBeurt);
            return true;
        }

        public Kleur OverwegendeKleur()
        {
            Dictionary<Kleur, int> score = new Dictionary<Kleur, int>()
            {
                { Kleur.Geen, 0},
                { Kleur.Wit, 0},
                { Kleur.Zwart, 0}
            };

            for (int y = 0; y < BordGrootte; y++)
            {
                for (int x = 0; x < BordGrootte; x++)
                {
                    score[Bord[y, x]]++;
                }
            }

            if (score[Kleur.Wit] == score[Kleur.Zwart])
            {
                return Kleur.Geen;
            }

            return score[Kleur.Wit] > score[Kleur.Zwart] ? Kleur.Wit : Kleur.Zwart;
        }

        public bool Pas()
        {
            if (GetMoves(AandeBeurt).Count() == 0)
            {
                AandeBeurt = ReverseKleur(AandeBeurt);
                return true;
            }

            return false;
        }

        public bool ZetMogelijk(int rijZet, int kolomZet) => ZetMogelijk(rijZet, kolomZet, null);
        private bool ZetMogelijk(int rijZet, int kolomZet, Kleur? force)
        {
            if (!ZetBinnenBord(rijZet, kolomZet))
            {
                return false;
            }

            if (Bord[rijZet, kolomZet] != Kleur.Geen)
            {
                return false;
            }

            if (force == null)
            {
                force = AandeBeurt;
            }

            bool valid = false;
            for (int y = -1; y <= 1 && !valid; y++)
            {
                for (int x = -1; x <= 1 && !valid; x++)
                {
                    valid = ControleerSequence(rijZet, kolomZet, x, y, (Kleur)force);
                }
            }

            return valid;
        }

        public static Kleur ReverseKleur(Kleur kleur)
        {
            return kleur switch
            {
                Kleur.Wit => Kleur.Zwart,
                Kleur.Zwart => Kleur.Wit,
                _ => Kleur.Geen,
            };
        }

        private bool ControleerSequence(int rijZet, int kolomZet, int x, int y, Kleur force, bool first = true)
        {
            if (!ZetBinnenBord(rijZet + y, kolomZet + x) || Bord[rijZet + y, kolomZet + x] == Kleur.Geen || (Bord[rijZet + y, kolomZet + x] == force && first))
            {
                return false;
            }

            if (Bord[rijZet + y, kolomZet + x] == ReverseKleur(force))
            {
                return ControleerSequence(rijZet + y, kolomZet + x, x, y, force, false);
            }

            return true;
        }

        private void UpdateSequence(int rijZet, int kolomZet, int x, int y)
        {
            if (Bord[rijZet + y, kolomZet + x] == ReverseKleur(AandeBeurt))
            {
                Bord[rijZet + y, kolomZet + x] = AandeBeurt;
                UpdateSequence(rijZet + y, kolomZet + x, x, y);
            }
        }

        private bool ZetBinnenBord(int rijZet, int kolomZet)
        {
            return rijZet < BordGrootte && rijZet >= 0 && kolomZet < BordGrootte && kolomZet >= 0;
        }
    }
}
