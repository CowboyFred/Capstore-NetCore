using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStore.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required] 
        [StringLength(50, ErrorMessage = "Supplier name cannot be longer than 50 characters.")] 
        public string Name { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string HomePhone { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string WorkPhone { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        public List<Cap> Caps { get; set; }
        public Supplier()
        {
            Caps = new List<Cap>();
        }
    }
}
