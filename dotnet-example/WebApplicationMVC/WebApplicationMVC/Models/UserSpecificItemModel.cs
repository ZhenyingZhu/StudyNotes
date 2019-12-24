using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationMVC.Models
{
    public class UserSpecificItemModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserID { get; set; }

        public IdentityUser StoreUser { get; set; }
    }
}
