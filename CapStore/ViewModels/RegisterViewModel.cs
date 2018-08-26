using System.ComponentModel.DataAnnotations;

namespace CapStore.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Your email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Your Username")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "Your First Name")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [Display(Name = "Your Last Name")]
        public string LastName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Your contact phone number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Phone]
        [Display(Name = "Your home phone number")]
        [DataType(DataType.PhoneNumber)]
        public string HomePhone { get; set; }

        [Phone]
        [Display(Name = "Your work phone number")]
        [DataType(DataType.PhoneNumber)]
        public string WorkPhone { get; set; }

        [Required]
        [Display(Name = "Delivery address")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Create Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password does not match the confirm password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm your password")]
        public string PasswordConfirm { get; set; }
    }
}
