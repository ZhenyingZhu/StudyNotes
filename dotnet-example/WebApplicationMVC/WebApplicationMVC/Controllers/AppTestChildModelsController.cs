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
    public class AppTestChildModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppTestChildModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppTestChildModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppTestChildModels.ToListAsync());
        }

        // GET: AppTestChildModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appTestChildModel = await _context.AppTestChildModels
                .Include(a => a.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appTestChildModel == null)
            {
                return NotFound();
            }

            return View(appTestChildModel);
        }

        // GET: AppTestChildModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppTestChildModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] AppTestChildModel appTestChildModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appTestChildModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appTestChildModel);
        }

        // GET: AppTestChildModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var appTestChildModel = await _context.AppTestChildModels.FindAsync(id);
            var appTestChildModel = await _context.AppTestChildModels.Include(a => a.Parent).SingleAsync(a => a.Id == id);
            if (appTestChildModel == null)
            {
                return NotFound();
            }
            return View(appTestChildModel);
        }

        // POST: AppTestChildModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] AppTestChildModel appTestChildModel)
        {
            if (id != appTestChildModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // zhenying: Pass in Parent id here as well, and use it to set up it.


                    _context.Update(appTestChildModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppTestChildModelExists(appTestChildModel.Id))
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
            return View(appTestChildModel);
        }

        // GET: AppTestChildModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appTestChildModel = await _context.AppTestChildModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appTestChildModel == null)
            {
                return NotFound();
            }

            return View(appTestChildModel);
        }

        // POST: AppTestChildModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appTestChildModel = await _context.AppTestChildModels.FindAsync(id);
            _context.AppTestChildModels.Remove(appTestChildModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppTestChildModelExists(int id)
        {
            return _context.AppTestChildModels.Any(e => e.Id == id);
        }
    }
}
