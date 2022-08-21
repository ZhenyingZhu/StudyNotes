using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Areas.Identity.Data;

namespace TodoApi.Models
{
    public class TodoRepository
    {
        private readonly TodoContext _context;
        private readonly UserManager<TodoApiUser> _userManager;
        private readonly ILogger<TodoContext> _logger;

        public TodoRepository(TodoContext context, UserManager<TodoApiUser> userManager, ILogger<TodoContext> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        private async Task<string> GetUserIdAsync()
        {
            TodoApiUser user = await _userManager.GetUserAsync(User);
            return user.Id;
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

        public async Task UpdateTodoItemAsync(TodoItem updatedTodoItem)
        {
            // Accept TodoItem so the Secret can be updated.
            var existingTodoItem = await _context.TodoItems
                .Where(t => t.Id == updatedTodoItem.Id)
                .Include(t => t.Project).FirstOrDefaultAsync();

            if (existingTodoItem == null)
            {
                throw new ObjectNotFoundException($"TodoItem {updatedTodoItem.Id} doesn't exist");
            }

            // Doesn't support move it to a different project. The operation will be done by another API.
            if (updatedTodoItem.ProjectId != null && updatedTodoItem.ProjectId != existingTodoItem.ProjectId)
            {
                throw new InvalidOperationException($"Cannot update project for TodoItem {updatedTodoItem.Id}");
            }

            existingTodoItem.Name = updatedTodoItem.Name;
            existingTodoItem.IsComplete = updatedTodoItem.IsComplete;
            existingTodoItem.Secret = updatedTodoItem.Secret;

            _context.Entry(existingTodoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(updatedTodoItem.Id))
            {
                // TodoItem get deleted concurrently.
                throw new ObjectNotFoundException($"TodoItem {updatedTodoItem.Id} doesn't exist");
            }
        }

        public async Task ChangeTodoItemProjectAsync(long tid, int pid)
        {
            var todoItem = await _context.TodoItems.FindAsync(tid);
            if (todoItem == null)
            {
                throw new ObjectNotFoundException($"TodoItem {tid} doesn't exist");
            }

            if (!ProjectExists(pid))
            {
                throw new ObjectNotFoundException($"Project {pid} doesn't exist");
            }

            todoItem.ProjectId = pid;
            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                if (!TodoItemExists(tid))
                {
                    throw new ObjectNotFoundException($"TodoItem {tid} doesn't exist");
                }

                if (!ProjectExists(pid))
                {
                    throw new ObjectNotFoundException($"Project {pid} doesn't exist");
                }
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
        [Obsolete]
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
        public async Task<ProjectDTO> CreateProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new ProjectDTO(project);
        }

        public async Task<List<ProjectDTO>> GetProjectsAsync()
        {
            // Use the DTO to avoid getting TodoItems then Projects in a loop.
            return await _context.Projects.Include(p => p.TodoItems).Select(p => new ProjectDTO(p)).ToListAsync();
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(long id)
        {
            // Cannot use FindAsync because Include(TodoItems) make the return not Project
            // var project = await _context.Projects.FindAsync(id);
            var project = await _context.Projects
                .Where(p => p.Id == id)
                .Include(p => p.TodoItems)
                .Select(p => new ProjectDTO(p)).FirstOrDefaultAsync();
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
            // catch exception: nted
            // Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes.See the inner exception for details.
            // Microsoft.Data.SqlClient.SqlException(0x80131904): The DELETE statement conflicted with the REFERENCE constraint "FK_TodoItems_Projects_ProjectId".The conflict occurred in database "TodoApi", table "dbo.TodoItems", column 'ProjectId'.
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<ProjectDTO> AddTodoItemToProjectAsync(int pid, TodoItem todoItem)
        {
            if (!ProjectExists(pid))
            {
                throw new ObjectNotFoundException($"Project {pid} doesn't exist");
            }

            if (todoItem.Id != 0)
            {
                _logger.LogWarning($"Creating a todoItem {todoItem.Name} with id set {todoItem.Id}");
            }

            todoItem.ProjectId = pid;
            _context.TodoItems.Add(todoItem);

            try
            {
                await _context.SaveChangesAsync();

                var project = await GetProjectByIdAsync(pid);
                if (project == null)
                {
                    throw new ObjectNotFoundException($"Project {pid} doesn't exist");
                }

                return project;
            }
            catch (DBConcurrencyException) when (!ProjectExists(pid))
            {
                throw new ObjectNotFoundException($"Project {pid} doesn't exist");
            }
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(p => p.Id == id);
        }
        #endregion
    }
}
