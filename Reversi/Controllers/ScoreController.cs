using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{
    public class ScoreController : Controller
    {
        private readonly ReversiMvcAppContext _context;
        private readonly UserManager<Speler> _userManager;

        public ScoreController(ReversiMvcAppContext context, UserManager<Speler> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Speler")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var scoreViewModel = new List<ScoreViewModel>();
            foreach (Speler user in users)
            {
                var thisViewModel = new ScoreViewModel();
                thisViewModel.Speler = user.Name;
                thisViewModel.AantalGewonnen = user.Won;
                thisViewModel.AantalVerloren = user.Lost;
                thisViewModel.AantalGelijk = user.Draw;
                scoreViewModel.Add(thisViewModel);
            }
            scoreViewModel.OrderBy(x => x.AantalGewonnen);
            return View(scoreViewModel);
        }
    }
}
