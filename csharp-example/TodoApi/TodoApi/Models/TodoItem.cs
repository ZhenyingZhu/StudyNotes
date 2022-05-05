using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public bool IsComplete { get; set; }

        public string? Secret { get; set; }

        public long? ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
