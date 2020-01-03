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
            PopulateParentsDropDownList();

            return View();
        }

        // POST: AppTestChildModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ParentID")] AppTestChildModel appTestChildModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appTestChildModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateParentsDropDownList(appTestChildModel.ParentID);
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

            PopulateParentsDropDownList(appTestChildModel.ParentID);
            return View(appTestChildModel);
        }

        // POST: AppTestChildModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParentID")] AppTestChildModel appTestChildModel)
        {
            if (id != appTestChildModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        private void PopulateParentsDropDownList(object selectParent = null)
        {
            // zhenying: add this to show all the existing parents.
            var parentsQuery = from appTestModel in _context.AppTestModel orderby appTestModel.AppTestInput select appTestModel;
            // DataTextField is what the user can see. DataValueField is what you can use for identify which one is selected from DropDownList.
            // The Id here is actually the Id of AppTestModel
            ViewBag.ParentID = new SelectList(parentsQuery.AsNoTracking(), "Id", "AppTestInput", selectParent);
        }
    }
}
