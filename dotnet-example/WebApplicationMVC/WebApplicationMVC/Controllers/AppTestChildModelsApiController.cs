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
    public class AppTestChildModelsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppTestChildModelsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AppTestChildModelsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppTestChildModel>>> GetAppTestChildModels()
        {
            return await _context.AppTestChildModels
                .Include(a => a.Parent)
                .ToListAsync();
        }

        // GET: api/AppTestChildModelsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppTestChildModel>> GetAppTestChildModel(int id)
        {
            //var appTestChildModel = await _context.AppTestChildModels.FindAsync(id);
            var appTestChildModel = await _context.AppTestChildModels
                .Include(a => a.Parent)
                .SingleAsync(a => a.Id == id);

            if (appTestChildModel == null)
            {
                return NotFound();
            }

            return appTestChildModel;
        }

        // PUT: api/AppTestChildModelsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppTestChildModel(int id, AppTestChildModel appTestChildModel)
        {
            if (id != appTestChildModel.Id)
            {
                return BadRequest();
            }

            if (appTestChildModel.Parent != null)
            {
                var parent = appTestChildModel.Parent;

                var existParent = await _context.AppTestModel.FindAsync(parent.Id);
                appTestChildModel.Parent = existParent;
            }

            _context.Entry(appTestChildModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppTestChildModelExists(id))
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

        // POST: api/AppTestChildModelsApi
        [HttpPost]
        public async Task<ActionResult<AppTestChildModel>> PostAppTestChildModel(AppTestChildModel appTestChildModel)
        {
            if (ModelState.IsValid)
            {
                // zhenying: this can add the parent to child and the parent also added with the child
                //if (appTestChildModel.Parent != null)
                //{
                //    var parent = appTestChildModel.Parent;

                //    var existParent = await _context.AppTestModel.FindAsync(parent.Id);
                //    appTestChildModel.Parent = existParent;
                //}

                _context.AppTestChildModels.Add(appTestChildModel);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAppTestChildModel", new { id = appTestChildModel.Id }, appTestChildModel);
            }
            else
            {
                // upper level thrown error so this part doesn't seem like would be called.
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/AppTestChildModelsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AppTestChildModel>> DeleteAppTestChildModel(int id)
        {
            var appTestChildModel = await _context.AppTestChildModels.FindAsync(id);
            if (appTestChildModel == null)
            {
                return NotFound();
            }

            _context.AppTestChildModels.Remove(appTestChildModel);
            await _context.SaveChangesAsync();

            return appTestChildModel;
        }

        private bool AppTestChildModelExists(int id)
        {
            return _context.AppTestChildModels.Any(e => e.Id == id);
        }
    }
}
