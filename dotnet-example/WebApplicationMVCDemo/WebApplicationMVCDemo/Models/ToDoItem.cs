using System;
using System.Collections.Generic;
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
    }
}
