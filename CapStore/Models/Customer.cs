using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace CapStore.Models
{
    public class Customer : IdentityUser
    {
        [Required(ErrorMessage = "Please enter your address")]
        [StringLength(300)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string HomePhone { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string WorkPhone { get; set; }

        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName { get; set; }
    }
}
