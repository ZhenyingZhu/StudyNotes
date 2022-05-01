using System;
using System.Collections.Generic;
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
            this._context = context;
            this._logger = logger;
        }

        public async Task<List<TodoItemDTO>> GetTodoItemsAsync()
        {
            // Need to return List, so the ActionResult can convert it to IEnumerable.
            return await this._context.TodoItems.Select(t => new TodoItemDTO(t)).ToListAsync();
        }

        public async Task<TodoItemDTO> GetTodoItemByIdAsync()
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return null;
            }

            return ItemToDTO(todoItem);
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
    }
}
