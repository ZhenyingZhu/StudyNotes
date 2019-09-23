using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationMVC.Models
{
    public class AppTestModel
    {
        [Key]
        public int Id { get; set; }

        public string AppTestInput { get; set; }

        public ICollection<AppTestChildModel> Children { get; set; }
    }
}
