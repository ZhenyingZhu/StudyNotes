using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class TodoRepository
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoContext> _logger;

        public TodoRepository(TodoContext context, ILogger<TodoContext> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region TodoItem
        [Obsolete]
        public async Task<TodoItemDTO> CreateTodoItemAsync(TodoItemDTO todoItemDTO)
        {
            if (todoItemDTO.Id != 0)
            {
                _logger.LogWarning($"Creating a todoItem {todoItemDTO.Name} with id set {todoItemDTO.Id}");
            }

            TodoItem todoItem = new TodoItem
            {
                Name = todoItemDTO.Name,
                IsComplete = todoItemDTO.IsComplete
            };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // Create a new TodoItemDTO because the todoItem.id is set to the real value and needed by the caller.
            return new TodoItemDTO(todoItem);
        }

        public async Task<List<TodoItemDTO>> GetTodoItemsAsync()
        {
            // Need to return List, so the ActionResult can convert it to IEnumerable.
            return await _context.TodoItems.Include(t => t.Project).Select(t => new TodoItemDTO(t)).ToListAsync();
        }

        public async Task<TodoItemDTO?> GetTodoItemByIdAsync(long id)
        {
            // Include can be added in any order but add after where might load less data?
            var todoItem = await _context.TodoItems.Where(t => t.Id == id).Include(t => t.Project).FirstOrDefaultAsync();

            // Let upper layer handles the NotFound error.
            if (todoItem == null)
            {
                return null;
            }

            return new TodoItemDTO(todoItem);
        }

        public async Task UpdateTodoItemAsync(TodoItemDTO todoItemDTO)
        {
            var todoItem = await _context.TodoItems.Where(t => t.Id == todoItemDTO.Id).Include(t => t.Project).FirstOrDefaultAsync();

            if (todoItem == null)
            {
                throw new ObjectNotFoundException($"TodoItem {todoItemDTO.Id} doesn't exist");
            }

            // Doesn't support move it to a different project. The operation will be done by another API.
            if ((todoItemDTO.ProjectId != null && todoItemDTO.ProjectId != todoItem.ProjectId) || 
                (todoItemDTO.ProjectName != null && todoItemDTO.ProjectName != todoItem.Project?.Name))
            {
                throw new InvalidOperationException($"Cannot update project for TodoItem {todoItem.Id}");
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(todoItemDTO.Id))
            {
                // TodoItem get deleted concurrently.
                throw new ObjectNotFoundException($"TodoItem {todoItemDTO.Id} doesn't exist");
            }
        }

        public async Task DeleteTodoItemAsync(long id)
        {
            // No need to remove it from project, because the project table doesn't record the reference.
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                throw new ObjectNotFoundException($"TodoItem {id} doesn't exist");
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
        }

        // A better solution is to create a TodoItemDTO class and let TodoItem inherit from it.
        // Or use [ModelMetadataType(typeof(TodoItem))] annotation on the TodoItemDTO
        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
        #endregion

        #region Project
        public async Task<Project> CreateProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            // TODO: Need convert the TodoItem to TodoItemDTO
            return await _context.Projects.Include(p => p.TodoItems).ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(long id)
        {
            // Cannot use FindAsync because Include(TodoItems) make the return not Project
            // var project = await _context.Projects.FindAsync(id);
            var project = await _context.Projects.Where(p => p.Id == id).Include(p => p.TodoItems).FirstOrDefaultAsync();
            return project;
        }

        public async Task UpdateProjectAsync(Project project)
        {
            // This method can only update project properties not todoItems.
            // As it will be hard to distinguish if the task list is to updating an existing task or adding a new task.
            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException) when (!ProjectExists(project.Id))
            {
                throw new ObjectNotFoundException($"Project {project.Id} doesn't exist");
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                throw new ObjectNotFoundException($"Project {id} doesn't exist");
            }

            // TODO: when there are todo items refering it, how to handle it.
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Project> AddTodoItemToProjectAsync(int pid, TodoItemDTO todoItemDTO)
        {
            var project = await GetProjectByIdAsync(pid);
            if (project == null)
            {
                throw new ObjectNotFoundException($"Project {pid} doesn't exist");
            }

            if (todoItemDTO.Id != 0)
            {
                _logger.LogWarning($"Creating a todoItem {todoItemDTO.Name} with id set {todoItemDTO.Id}");
            }

            TodoItem todoItem = new TodoItem
            {
                Name = todoItemDTO.Name,
                IsComplete = todoItemDTO.IsComplete,
                ProjectId = pid
            };
            _context.TodoItems.Add(todoItem);

            try
            {
                await _context.SaveChangesAsync();

                // Should reload project?
                return project;
            }
            catch (DBConcurrencyException) when (!ProjectExists(pid))
            {
                throw new ObjectNotFoundException($"Project {pid} doesn't exist");
            }
        }

        public async Task MoveTodoItemToProjectAsync(int pid, int tid)
        {
            var todoItem = await _context.TodoItems.FindAsync(tid);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(p => p.Id == id);
        }
        #endregion
    }
}
