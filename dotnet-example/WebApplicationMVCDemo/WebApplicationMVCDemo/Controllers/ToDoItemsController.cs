using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationMVCDemo.Data;
using WebApplicationMVCDemo.Models;

namespace WebApplicationMVCDemo.Controllers
{
    [Authorize]
    public class ToDoItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public ToDoItemsController(ApplicationDbContext context, UserManager<IdentityUser> userNamager)
        {
            _context = context;
            _userManager = userNamager;
        }

        // GET: ToDoItems
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            return View(await _context.ToDoItems.Where(t => t.OwnerId == user.Id).ToListAsync());
        }

        // GET: ToDoItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IdentityUser user = await _userManager.GetUserAsync(User);

            var toDoItem = await _context.ToDoItems
                .FirstOrDefaultAsync(m => m.Id == id && m.OwnerId == user.Id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // GET: ToDoItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OwnerId,Title,State,DueDate")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.GetUserAsync(User);
                toDoItem.OwnerId = user.Id;
                _context.Add(toDoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoItem);
        }

        // GET: ToDoItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser user = await _userManager.GetUserAsync(User);
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null || toDoItem.OwnerId != user.Id)
            {
                return NotFound();
            }
            return View(toDoItem);
        }

        // POST: ToDoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,Title,State,DueDate")] ToDoItem toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return NotFound();
            }

            IdentityUser user = await _userManager.GetUserAsync(User);
            if (toDoItem.OwnerId != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoItemExists(toDoItem.Id))
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
            return View(toDoItem);
        }

        // GET: ToDoItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser user = await _userManager.GetUserAsync(User);
            var toDoItem = await _context.ToDoItems
                .FirstOrDefaultAsync(m => m.Id == id && m.OwnerId == user.Id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);

            IdentityUser user = await _userManager.GetUserAsync(User);
            if (toDoItem == null || toDoItem.OwnerId != user.Id)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(toDoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoItemExists(int id)
        {
            IdentityUser user = _userManager.GetUserAsync(User).Result;
            return _context.ToDoItems.Any(e => e.Id == id && e.OwnerId == user.Id);
        }
    }
}
