using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PawesomePalace.Models
{
    public class PetOwnerModel
    {
        [Key]
        public Guid OwnerId { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [MaxLength(20), Phone]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(50)]
        public string State { get; set; }

        [MaxLength(10)]
        public string ZipCode { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public List<EmergencyContactModel> EmergencyContacts { get; set; }
        public List<PetModel> Pets { get; set; }

        public PetOwnerModel()
        {
            OwnerId = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            EmergencyContacts = new List<EmergencyContactModel>();
            Pets = new List<PetModel>();
        }
    }
}