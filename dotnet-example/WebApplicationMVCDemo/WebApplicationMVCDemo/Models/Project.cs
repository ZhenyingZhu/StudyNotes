using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationMVCDemo.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }

        [DisplayName("Project Title")]
        public string Title { get; set; }

        public List<ToDoItem> ToDos { get; set; }
    }
}
