using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationMVC.Data;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    [Authorize]
    public class AppTestModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppTestModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppTestModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppTestModel.ToListAsync());
        }

        // GET: AppTestModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appTestModel = await _context.AppTestModel
                .Include(a => a.Children)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (appTestModel == null)
            {
                return NotFound();
            }

            return View(appTestModel);
        }

        // GET: AppTestModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppTestModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppTestInput")] AppTestModel appTestModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appTestModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appTestModel);
        }

        // GET: AppTestModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appTestModel = await _context.AppTestModel.FindAsync(id);
            if (appTestModel == null)
            {
                return NotFound();
            }
            return View(appTestModel);
        }

        // POST: AppTestModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppTestInput")] AppTestModel appTestModel)
        {
            if (id != appTestModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appTestModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppTestModelExists(appTestModel.Id))
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
            return View(appTestModel);
        }

        // GET: AppTestModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appTestModel = await _context.AppTestModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appTestModel == null)
            {
                return NotFound();
            }

            return View(appTestModel);
        }

        // POST: AppTestModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appTestModel = await _context.AppTestModel.FindAsync(id);
            _context.AppTestModel.Remove(appTestModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppTestModelExists(int id)
        {
            return _context.AppTestModel.Any(e => e.Id == id);
        }
    }
}
