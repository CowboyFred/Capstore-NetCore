using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Category name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
