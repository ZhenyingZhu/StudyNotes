namespace TodoApi.Models
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<TodoItemDTO>? TodoItems { get; set; }

        public ProjectDTO()
        {
        }

        public ProjectDTO(Project project)
        {
            Id = project.Id;
            Name = project.Name;
            TodoItems = project?.TodoItems?.Select(t => new TodoItemDTO(t)).ToList();
        }
    }
}
