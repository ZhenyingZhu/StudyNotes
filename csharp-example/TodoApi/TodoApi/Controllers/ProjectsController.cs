#nullable disable
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly TodoRepository _repository;

        public ProjectsController(TodoRepository repository, ILogger<ProjectsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _repository.GetProjectsAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _repository.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateProjectAsync(project);

                return NoContent();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            await _repository.CreateProjectAsync(project);

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                await _repository.DeleteProjectAsync(id);

                return NoContent();
            }
            catch(ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Projects/5/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{pid}/TodoItems")]
        public async Task<ActionResult<Project>> AddTodoItemToProject(int pid, TodoItemDTO todoItemDTO)
        {
            // https://stackoverflow.com/questions/48359363/ef-core-adding-updating-entity-and-adding-updating-removing-child-entities-in
            try
            {
                var project = await _repository.AddTodoItemToProjectAsync(pid, todoItemDTO);

                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
