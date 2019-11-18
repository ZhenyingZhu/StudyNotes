using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationMVC.Data;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    public class UserSpecificItemModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserSpecificItemModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserSpecificItemModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserSpecificItemModel.ToListAsync());
        }

        // GET: UserSpecificItemModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userSpecificItemModel = await _context.UserSpecificItemModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userSpecificItemModel == null)
            {
                return NotFound();
            }

            return View(userSpecificItemModel);
        }

        // GET: UserSpecificItemModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserSpecificItemModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] UserSpecificItemModel userSpecificItemModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userSpecificItemModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userSpecificItemModel);
        }

        // GET: UserSpecificItemModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userSpecificItemModel = await _context.UserSpecificItemModel.FindAsync(id);
            if (userSpecificItemModel == null)
            {
                return NotFound();
            }
            return View(userSpecificItemModel);
        }

        // POST: UserSpecificItemModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] UserSpecificItemModel userSpecificItemModel)
        {
            if (id != userSpecificItemModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userSpecificItemModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserSpecificItemModelExists(userSpecificItemModel.Id))
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
            return View(userSpecificItemModel);
        }

        // GET: UserSpecificItemModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userSpecificItemModel = await _context.UserSpecificItemModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userSpecificItemModel == null)
            {
                return NotFound();
            }

            return View(userSpecificItemModel);
        }

        // POST: UserSpecificItemModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userSpecificItemModel = await _context.UserSpecificItemModel.FindAsync(id);
            _context.UserSpecificItemModel.Remove(userSpecificItemModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserSpecificItemModelExists(int id)
        {
            return _context.UserSpecificItemModel.Any(e => e.Id == id);
        }
    }
}
