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
        public async Task<List<TodoItemDTO>> GetTodoItemsAsync()
        {
            // Need to return List, so the ActionResult can convert it to IEnumerable.
            return await _context.TodoItems.Select(t => new TodoItemDTO(t)).ToListAsync();
        }

        public async Task<TodoItemDTO?> GetTodoItemByIdAsync(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            // Let upper layer handles the NotFound error.
            if (todoItem == null)
            {
                return null;
            }

            return new TodoItemDTO(todoItem);
        }

        public async Task UpdateTodoItemAsync(TodoItemDTO todoItemDTO)
        {
            var todoItem = await _context.TodoItems.FindAsync(todoItemDTO.Id);

            if (todoItem == null)
            {
                throw new ObjectNotFoundException($"TodoItem {todoItemDTO.Id} doesn't exist");
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
                throw new ObjectNotFoundException($"TodoItem {todoItemDTO.Id} doesn't exist");
            }
        }

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

            // Create a new TodoItemDTO because the todoItem.id is set and needed by the caller.
            return new TodoItemDTO(todoItem);
        }

        public async Task DeleteTodoItemAsync(long id)
        {
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


    }
}
