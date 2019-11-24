using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationMVC.Data;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UserSpecificItemModelsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserSpecificItemModelsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserSpecificItemModelsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSpecificItemModel>>> GetUserSpecificItemModel()
        {
            return await _context.UserSpecificItemModel.ToListAsync();
        }

        // GET: api/UserSpecificItemModelsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserSpecificItemModel>> GetUserSpecificItemModel(int id)
        {
            var userSpecificItemModel = await _context.UserSpecificItemModel.FindAsync(id);

            if (userSpecificItemModel == null)
            {
                return NotFound();
            }

            return userSpecificItemModel;
        }

        // PUT: api/UserSpecificItemModelsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserSpecificItemModel(int id, UserSpecificItemModel userSpecificItemModel)
        {
            if (id != userSpecificItemModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(userSpecificItemModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserSpecificItemModelExists(id))
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

        // POST: api/UserSpecificItemModelsApi
        [HttpPost]
        public async Task<ActionResult<UserSpecificItemModel>> PostUserSpecificItemModel(UserSpecificItemModel userSpecificItemModel)
        {
            _context.UserSpecificItemModel.Add(userSpecificItemModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserSpecificItemModel", new { id = userSpecificItemModel.Id }, userSpecificItemModel);
        }

        // DELETE: api/UserSpecificItemModelsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserSpecificItemModel>> DeleteUserSpecificItemModel(int id)
        {
            var userSpecificItemModel = await _context.UserSpecificItemModel.FindAsync(id);
            if (userSpecificItemModel == null)
            {
                return NotFound();
            }

            _context.UserSpecificItemModel.Remove(userSpecificItemModel);
            await _context.SaveChangesAsync();

            return userSpecificItemModel;
        }

        private bool UserSpecificItemModelExists(int id)
        {
            return _context.UserSpecificItemModel.Any(e => e.Id == id);
        }
    }
}
