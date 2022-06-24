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
    public class MessageController : ControllerBase
    {

        private readonly ReversiMvcAppContext _context;
        private readonly IHubContext<ReversiMvcAppHub> _hubContext;

        public MessageController(ReversiMvcAppContext context, IHubContext<ReversiMvcAppHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // POST: api/Message/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("{id}")]
        public async Task<IActionResult> PostMessage(string id, [FromBody] string content)
        {
            SpelSpeler spelSpeler = await _context.SpelSpelers.FirstOrDefaultAsync(ss => ss.SpelId == id && ss.Spel.SpelState != SpelState.Waiting && ss.Speler.UserName == HttpContext.User.Identity.Name);
            if (spelSpeler == null)
            {
                return Unauthorized();
            }

            Message message = new Message()
            {
                Content = content,
                Type = MessageType.Message,
                SpelSpelerId = spelSpeler.Id
            };

            _context.Messages.Add(message);

            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.Group(spelSpeler.SpelId).SendAsync("Message", message);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return NoContent();
        }
    }
}