using OnlineStore.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public class User
    {
        public int UserId { get; set; } = 0;

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        [Range(0,100, ErrorMessage = "Range Validation failed")]
        [CustomAgeValidator(ErrorMessage = "Custom age validation failed")]
        public int age { get; set; }

    }
}
