using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Models
{
    [ModelMetadataType(typeof(TodoItem))]
    public class TodoItemDTO
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public bool IsComplete { get; set; }

        public long? ProjectId { get; set; }

        public string? ProjectName { get; set; }

        public TodoItemDTO()
        {
        }

        public TodoItemDTO(TodoItem todoItem)
        {
            this.Id = todoItem.Id;
            this.Name = todoItem.Name;
            this.IsComplete = todoItem.IsComplete;
            this.ProjectId = todoItem.ProjectId;
            this.ProjectName = todoItem.Project?.Name;
        }
    }
}
