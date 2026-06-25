using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PawesomePalace.Models
{
    public class EmployeeModel
    {
        [Key]
        public Guid EmployeeId { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [MaxLength(20), Phone]
        public string Phone { get; set; }

        public EmployeeModel()
        {
            EmployeeId = Guid.NewGuid();
        }
    }
}