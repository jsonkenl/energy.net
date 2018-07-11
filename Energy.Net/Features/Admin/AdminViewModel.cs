using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Energy.Net.Features.Admin
{
    public class AdminViewModel
    {
        // passwordOption1 = "^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])"
        // passwordOption2 = "(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])"
        // passwordOption3 = "(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])"
        // passwordOption4 = "(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$"

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters long", MinimumLength = 8)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", 
            ErrorMessage = "Passwords must contain at least 3 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
