using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models
{
    public class EmergencyContactModel
    {
        [Key]
        public Guid EmergencyContactId { get; set; }

        [Required]
        public Guid OwnerId { get; set; } // FK

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(20), Phone]
        public string Phone { get; set; }

        [Required, MaxLength(50)]
        public string Relationship { get; set; }

        // Navigational Properties
        [ForeignKey("OwnerId")]
        public PetOwnerModel Owner { get; set; }

        public EmergencyContactModel()
        {
            EmergencyContactId = Guid.NewGuid();
        }
    }
}