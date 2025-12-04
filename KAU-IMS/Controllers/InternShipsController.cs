using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KAU_IMS.Data;
using KAU_IMS.Models;

namespace KAU_IMS.Controllers
{
    public class InternShipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InternShipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InternShips
        public async Task<IActionResult> Index()
        {
            return View(await _context.InternShip.ToListAsync());
        }

        // GET: InternShips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internShip = await _context.InternShip
                .FirstOrDefaultAsync(m => m.id == id);
            if (internShip == null)
            {
                return NotFound();
            }

            return View(internShip);
        }

        // GET: InternShips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InternShips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title,internship_company,n_skills,description,specialty")] InternShip internShip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(internShip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(internShip);
        }

        // GET: InternShips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internShip = await _context.InternShip.FindAsync(id);
            if (internShip == null)
            {
                return NotFound();
            }
            return View(internShip);
        }

        // POST: InternShips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title,internship_company,n_skills,description,specialty")] InternShip internShip)
        {
            if (id != internShip.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(internShip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternShipExists(internShip.id))
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
            return View(internShip);
        }

        // GET: InternShips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internShip = await _context.InternShip
                .FirstOrDefaultAsync(m => m.id == id);
            if (internShip == null)
            {
                return NotFound();
            }

            return View(internShip);
        }

        // POST: InternShips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var internShip = await _context.InternShip.FindAsync(id);
            if (internShip != null)
            {
                _context.InternShip.Remove(internShip);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InternShipExists(int id)
        {
            return _context.InternShip.Any(e => e.id == id);
        }
    }
}
