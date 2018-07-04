using System;
using System.Collections.Generic;
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
        public IList<Role> Roles { get; set; }

        [Phone]
        public string Direct { get; set; }

        [Phone]
        public string Mobile { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImagePath { get; set; }

        [DataType(DataType.Date), DisplayName("Birthday")]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
    }
}
