using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data;
using ReversiMvcApp.Hubs;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpelController : ControllerBase
    {
        private readonly ReversiMvcAppContext _context;
        private readonly IHubContext<ReversiMvcAppHub> _hubContext;

        public SpelController(ReversiMvcAppContext context, IHubContext<ReversiMvcAppHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public class SpelResult
        {
            public object Spel { get; set; }
            public IEnumerable<object> Spelers { get; set; }
            public Kleur Color { get; set; }

            public SpelResult(SpelSpeler spelSpeler) { 

                Spel spel = spelSpeler.Spel;

                Spel = new
                {
                    id = spel.Id,
                    description = spel.Omschrijving,
                    size = spel.BordGrootte,
                    date = spel.DateCreate,
                    state = spel.SpelState,
                    bord = spel.GetBord(),
                    history = spel.Coordinates.OrderBy(h => h.Order),
                    moves = spel.GetMoves(spelSpeler.Kleur),
                    turn = spel.AandeBeurt
                };

                Spelers = spel.SpelSpelers.Select(ss => new
                {
                    name = ss.Speler.Name,
                    color = ss.Kleur,
                    won = ss.Speler.Won,
                    lost = ss.Speler.Lost,
                    draw = ss.Speler.Draw,
                    forfeit = ss.Speler.Forfeit,
                    sum = ss.Speler.Sum
                });

                Color = spelSpeler.Kleur;
            }
        }

        // GET: api/Spel
        [HttpGet]
        public async Task<ActionResult<SpelResult>> GetSpel()
        {
            SpelSpeler spelSpeler = await _context.SpelSpelers.FirstOrDefaultAsync(ss => ss.Spel.SpelState == SpelState.Ongoing && ss.Speler.UserName == HttpContext.User.Identity.Name);
            if (spelSpeler == null)
            {
                return BadRequest();
            }

            return new SpelResult(spelSpeler);
        }


        // POST: api/Spel
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<IActionResult> PostSpel(Coordinate coordinate)
        {
            SpelSpeler spelSpeler = await _context.SpelSpelers.FirstOrDefaultAsync(ss => ss.Spel.SpelState == SpelState.Ongoing && ss.Speler.UserName == HttpContext.User.Identity.Name);
            if (spelSpeler == null || spelSpeler.Kleur != spelSpeler.Spel.AandeBeurt)
            {
                return Unauthorized();
            }

            if (!spelSpeler.Spel.DoeZet(coordinate.Y, coordinate.X))
            {
                return BadRequest();
            }

            bool afgelopen = spelSpeler.Spel.Afgelopen();

            try
            {
                await _context.SaveChangesAsync();
                if (afgelopen)
                {
                    await _hubContext.Clients.Group(spelSpeler.SpelId).SendAsync("Finish", new SpelResult(spelSpeler) { 
                        Color = Kleur.Geen
                    });
                }
                else
                {
                    await _hubContext.Clients.Group(spelSpeler.SpelId).SendAsync("Move");
                }
               
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return NoContent();
        }


        // PUT: api/Spel
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public async Task<IActionResult> PutSpel()
        {
            SpelSpeler spelSpeler = await _context.SpelSpelers.FirstOrDefaultAsync(ss => ss.Spel.SpelState == SpelState.Ongoing && ss.Speler.UserName == HttpContext.User.Identity.Name);
            //Scores spelerscores1 = await _context.Scores.FirstOrDefaultAsync(ss => ss.Speler == HttpContext.User.Identity.Name);
            //Scores spelerscores2 = await _context.Scores.FirstOrDefaultAsync(ss => ss.Speler != HttpContext.User.Identity.Name);

            //if (spelerscores1 == null)
            //{
            //    Scores score = new Scores();
            //    score.Speler = HttpContext.User.Identity.Name;
            //    score.AantalGewonnen = 0;
            //    score.AantalVerloren = 0;
            //    score.AantalGelijk = 0;

            //    await _context.Scores.AddAsync(score);
            //}

            if (spelSpeler == null || spelSpeler.Kleur != spelSpeler.Spel.AandeBeurt)
            {
                return Unauthorized();
            }

            if (!spelSpeler.Spel.Pas())
            {
                return BadRequest();
            }

            bool afgelopen = spelSpeler.Spel.Afgelopen();

            try
            {
                await _context.SaveChangesAsync();
                if (afgelopen)
                {
                    //Scores score = await _context.Scores.FirstOrDefaultAsync(ss => ss.Speler == HttpContext.User.Identity.Name);
                    
                    //score.AantalGelijk += spelSpeler.Speler.Draw;
                    //score.AantalGewonnen += spelSpeler.Speler.Won;
                    //score.AantalVerloren += spelSpeler.Speler.Lost;
                    //await _context.SaveChangesAsync();
                    await _hubContext.Clients.Group(spelSpeler.SpelId).SendAsync("Finish", new SpelResult(spelSpeler)
                    {
                        Color = Kleur.Geen
                    });
                }
                else
                {
                    await _hubContext.Clients.Group(spelSpeler.SpelId).SendAsync("Move");
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return NoContent();
        }


        // DELETE: api/Spel
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteSpel()
        {
            SpelSpeler spelSpeler = await _context.SpelSpelers.FirstOrDefaultAsync(ss => ss.Speler.UserName == HttpContext.User.Identity.Name && ss.Spel.SpelState == SpelState.Ongoing);

            if (spelSpeler == null)
            {
                return BadRequest();
            }

            if (spelSpeler.Kleur == Kleur.Wit)
            {
                spelSpeler.Spel.SpelState = SpelState.ForfeitWit;
            } 
            else if (spelSpeler.Kleur == Kleur.Zwart)
            {
                spelSpeler.Spel.SpelState = SpelState.ForfeitZwart;
            }
            else
            {
                return BadRequest();
            }

            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group(spelSpeler.SpelId).SendAsync("Finish", new SpelResult(spelSpeler)
            {
                Color = Kleur.Geen
            });

            return true;
        }
    }
}
