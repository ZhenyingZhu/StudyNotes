namespace TodoApi.Models
{
    public class Project
    {
        public int Id { get; set; }

        // Since CreateProject API doesn't use a DTO, and the OwnerId is not need to be passed in by user,
        // even it is required in DB, still need to set it as nullable so EF is happy.
        public string? OwnerId { get; set; }

        public string? Name { get; set; }

        public ICollection<TodoItem>? TodoItems { get; set; }
    }
}
