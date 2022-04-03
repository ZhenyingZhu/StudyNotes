using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Models
{
    [ModelMetadataType(typeof(TodoItem))]
    public class TodoItemDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
