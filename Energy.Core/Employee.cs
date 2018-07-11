using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Energy.Core
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public bool Contract { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required, DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        public string DistinguishedName { get; set; }

        [Required]
        public Role Role { get; set; }

        [Phone]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", 
            ErrorMessage = "10 characters, no dashes or special characters")]
        public string Direct { get; set; }

        [Phone]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", 
            ErrorMessage = "10 characters, no dashes or special characters")]
        public string Mobile { get; set; }

        [DataType(DataType.ImageUrl), DisplayName("Image Path")]
        public string ImagePath { get; set; }

        [DataType(DataType.Date), DisplayName("Birthday")]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
    }
}
