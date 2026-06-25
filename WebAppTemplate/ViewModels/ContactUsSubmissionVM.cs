using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PawesomePalace.ViewModels
{
    public class ContactUsSubmissionVM
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        [MaxLength(200)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }
    }
}