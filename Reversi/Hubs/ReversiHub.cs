using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Reversi.Data;
using Reversi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Reversi.Hubs
{
    [Authorize]
    public class ReversiHub : Hub
    {
        private readonly ReversiContext _context;

        public ReversiHub(ReversiContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            await SendGroup(true);
            await base.OnConnectedAsync();
        }

        public async Task Group() => await SendGroup(false);

        private async Task SendGroup(bool init)
        {
            string spelId = await _context.SpelSpelers.Where(ss => ss.Spel.SpelState == SpelState.Ongoing && ss.Speler.UserName == Context.GetHttpContext().User.Identity.Name).Select(ss => ss.SpelId).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(spelId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "lobby");

                var list = await _context.SpelSpelers.Where(ss => ss.Speler.UserName == Context.GetHttpContext().User.Identity.Name).Select(ss => ss.SpelId).ToListAsync();
                foreach (var item in list)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, item);
                }

                await Clients.User(Context.UserIdentifier).SendAsync(init ? "Init" : "Group", "lobby");
            }
            else
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "lobby");
                await Groups.AddToGroupAsync(Context.ConnectionId, spelId);
                await Clients.User(Context.UserIdentifier).SendAsync(init ? "Init" : "Group", spelId);
            }
        }
    }
}
