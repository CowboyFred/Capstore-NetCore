using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;
namespace CapStore.Models
{
    public class Cap
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Cap name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool HasImage()
        {
            return !String.IsNullOrWhiteSpace(Image);
        }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public bool Visible { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<OrderCap> OrderCaps { get; set; }
        public Cap()
        {
            OrderCaps = new List<OrderCap>();
        }

    }
}
