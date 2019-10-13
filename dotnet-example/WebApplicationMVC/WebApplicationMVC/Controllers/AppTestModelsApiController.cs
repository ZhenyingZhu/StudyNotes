using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationMVC.Data;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppTestModelsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppTestModelsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AppTestModelsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppTestModel>>> GetAppTestModel()
        {
            return await _context.AppTestModel.Include(t => t.Children).ToListAsync();
        }

        // GET: api/AppTestModelsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppTestModel>> GetAppTestModel(int id)
        {
            //var appTestModel = await _context.AppTestModel.FindAsync(id);
            var appTestModel = await _context.AppTestModel
                .Include(t => t.Children)
                .SingleAsync(a => a.Id == id);

            if (appTestModel == null)
            {
                return NotFound();
            }

            return appTestModel;
        }

        // PUT: api/AppTestModelsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppTestModel(int id, AppTestModel appTestModel)
        {
            if (id != appTestModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(appTestModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppTestModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AppTestModelsApi
        [HttpPost]
        public async Task<ActionResult<AppTestModel>> PostAppTestModel(AppTestModel appTestModel)
        {
            _context.AppTestModel.Add(appTestModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppTestModel", new { id = appTestModel.Id }, appTestModel);
        }

        // DELETE: api/AppTestModelsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AppTestModel>> DeleteAppTestModel(int id)
        {
            var appTestModel = await _context.AppTestModel.FindAsync(id);
            if (appTestModel == null)
            {
                return NotFound();
            }

            _context.AppTestModel.Remove(appTestModel);
            await _context.SaveChangesAsync();

            return appTestModel;
        }

        private bool AppTestModelExists(int id)
        {
            return _context.AppTestModel.Any(e => e.Id == id);
        }
    }
}
