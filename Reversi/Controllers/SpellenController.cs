using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Controllers
{
    [Authorize]
    public class SpellenController : Controller
    {
        private readonly ReversiMvcAppContext _context;

        public SpellenController(ReversiMvcAppContext context)
        {
            _context = context;
        }

        // GET: Spellen
        [Authorize(Roles = "Beheerder, Mediator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Spellen.ToListAsync());
        }

        // GET: Spellen/Details/5
        [Authorize(Roles = "Beheerder, Mediator")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spellen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }

        // GET: Spellen/Create
        [Authorize(Roles = "Beheerder, Mediator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Spellen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Omschrijving,AandeBeurt")] Spel spel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spel);
        }

        // GET: Spellen/Edit/5
        [Authorize(Roles = "Beheerder, Mediator")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spellen.FindAsync(id);
            if (spel == null)
            {
                return NotFound();
            }
            return View(spel);
        }

        // POST: Spellen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Beheerder, Mediator")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Omschrijving,AandeBeurt")] Spel spel)
        {
            if (id != spel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpelExists(spel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(spel);
        }

        // GET: Spellen/Delete/5
        [Authorize(Roles = "Beheerder, Mediator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spellen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }

        // POST: Spellen/Delete/5
        [Authorize(Roles = "Beheerder, Mediator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            List<SpelSpeler> spelers = await _context.SpelSpelers.Where(x => x.SpelId == id).ToListAsync();
            foreach (var item in spelers)
            {
                _context.SpelSpelers.Remove(item);
            }

            List<Coordinate> coordinates = await _context.Coordinates.Where(x => x.SpelId == id).ToListAsync();
            foreach (var item in coordinates)
            {
                _context.Coordinates.Remove(item);
            }

            var spel = await _context.Spellen.FindAsync(id);
            _context.Spellen.Remove(spel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpelExists(string id)
        {
            return _context.Spellen.Any(e => e.Id == id);
        }
    }
}
