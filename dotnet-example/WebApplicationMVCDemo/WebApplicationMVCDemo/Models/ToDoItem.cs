using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationMVCDemo.Models
{
    public enum TaskState
    {
        IsQueued,
        IsStarted,
        IsCompleted
    }

    public class ToDoItem
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }

        public string Title { get; set; }

        public TaskState State { get; set; }

        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-ddTHH:mm}")]
        public DateTime? DueDate { get; set; }

        // Project 1-to-many ToDoItem
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
