﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationMVC.Models
{
    public class AppTestChildModel
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3)]
        public string Name { get; set; }

        public int ParentID { get; set; }

        public AppTestModel Parent { get; set; }
    }
}
