using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data;
using ReversiMvcApp.Hubs;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        private readonly ReversiMvcAppContext _context;
        private readonly IHubContext<ReversiMvcAppHub> _hubContext;

        public LobbyController(ReversiMvcAppContext context, IHubContext<ReversiMvcAppHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/Lobby
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetLobby()
        {
            return await _context.SpelSpelers.Where(ss => ss.Spel.SpelState == SpelState.Waiting && _context.SpelSpelers.Count(sss => sss.SpelId == ss.SpelId) == 1).Select(ss => new { 
                spel = new {
                    id = ss.Spel.Id,
                    description = ss.Spel.Omschrijving,
                    size = ss.Spel.BordGrootte,
                    date = ss.Spel.DateCreate,
                    state = ss.Spel.SpelState
                },
                speler = new
                {
                    name = ss.Speler.Name,
                    won = ss.Speler.Won,
                    lost = ss.Speler.Lost,
                    draw = ss.Speler.Draw,
                    forfeit = ss.Speler.Forfeit,
                    sum = ss.Speler.Sum
                },
                participating = ss.Speler.UserName == HttpContext.User.Identity.Name
            }).ToListAsync();
        }

        // GET: api/Lobby/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetLobby(string id)
        {
            SpelSpeler spelSpeler = await _context.SpelSpelers.FirstOrDefaultAsync(ss => ss.SpelId == id && ss.Spel.SpelState == SpelState.Waiting && _context.SpelSpelers.Count(sss => sss.SpelId == ss.SpelId) == 1);

            return new
            {
                spel = new
                {
                    id = spelSpeler.Spel.Id,
                    description = spelSpeler.Spel.Omschrijving,
                    size = spelSpeler.Spel.BordGrootte,
                    date = spelSpeler.Spel.DateCreate,
                    state = spelSpeler.Spel.SpelState
                },
                speler = new
                {
                    name = spelSpeler.Speler.Name,
                    won = spelSpeler.Speler.Won,
                    lost = spelSpeler.Speler.Lost,
                    draw = spelSpeler.Speler.Draw,
                    forfeit = spelSpeler.Speler.Forfeit,
                    sum = spelSpeler.Speler.Sum
                },
                owner = spelSpeler.Speler.UserName == HttpContext.User.Identity.Name
            };
        }


        // PUT: api/Lobby/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLobby(string id)
        {
            Speler speler = await GetSpeler();
            Spel spel = await _context.Spellen.FindAsync(id);
            SpelSpeler opponent = await _context.SpelSpelers.Where(ss => ss.SpelId == spel.Id).FirstOrDefaultAsync();

            if (speler == null)
            {
                return Unauthorized();
            }

            if (spel == null || spel.SpelSpelers.Count() > 1 || opponent.SpelerId == speler.Id)
            {
                return BadRequest();
            }

            Kleur kleur = new Random(DateTime.Now.Millisecond).NextDouble() > 0.5 ? Kleur.Zwart : Kleur.Wit;

            spel.Start();
            spel.SpelState = SpelState.Ongoing;
            _context.SpelSpelers.Add(new SpelSpeler()
            {
                SpelId = spel.Id,
                SpelerId = speler.Id,
                Kleur = kleur
            });

            opponent.Kleur = Spel.ReverseKleur(kleur);

            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.Group("lobby").SendAsync("Start", spel.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }


        public class PostArgs
        {
            public string Description { get; set; }
            public int Size { get; set; }
        }

        // POST: api/Lobby
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<object>> PostLobby([FromBody] PostArgs postArgs)
        {
            Speler speler = await GetSpeler();
            if (speler == null)
            {
                return Unauthorized();
            }

            if (_context.SpelSpelers.Count(ss => ss.SpelerId == speler.Id && (ss.Spel.SpelState == SpelState.Waiting || ss.Spel.SpelState == SpelState.Ongoing)) > 0)
            {
                return Conflict();
            }

            Spel spel = new Spel()
            {
                Omschrijving = postArgs.Description ?? "",
                BordGrootte = postArgs.Size
            };
            SpelSpeler spelSpeler = new SpelSpeler()
            {
                SpelId = spel.Id,
                SpelerId = speler.Id
            };

            _context.Spellen.Add(spel);
            _context.SpelSpelers.Add(spelSpeler);

            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.Group("lobby").SendAsync("Update");
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return await GetLobby(spel.Id);
        }


        // DELETE: api/Lobby
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteLobby()
        {
            IEnumerable<SpelSpeler> spelSpelers = await _context.SpelSpelers.Where(ss => ss.Spel.SpelState == SpelState.Waiting && ss.Speler.UserName == HttpContext.User.Identity.Name).ToListAsync();
            if (spelSpelers.Count() < 1)
            {
                return BadRequest();
            }

            IEnumerable<Spel> spellen = spelSpelers.Select(s => s.Spel);

            _context.Spellen.RemoveRange(spellen);
            _context.SpelSpelers.RemoveRange(spelSpelers);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group("lobby").SendAsync("Update");


            return true;
        }


        private async Task<Speler> GetSpeler() => await _context.Spelers.Where(s => s.UserName == HttpContext.User.Identity.Name).FirstOrDefaultAsync();
    }
}
