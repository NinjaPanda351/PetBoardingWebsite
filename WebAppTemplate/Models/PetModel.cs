using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppTemplate.Models
{
    public class PetModel
    {
        [Key]
        public Guid PetId { get; set; }

        [Required]
        public Guid OwnerId { get; set; } // FK

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Breed { get; set; }

        [Range(0, 30)]
        public int Age { get; set; }

        [MaxLength(50)]
        public string Color { get; set; }

        [MaxLength(50)]
        public string SecondaryColor { get; set; }

        [MaxLength(100)]
        public string VetName { get; set; }

        [MaxLength(20), Phone]
        public string VetPhone { get; set; }

        [MaxLength(1000)]
        public string MedicalNotes { get; set; }

        [MaxLength(500)]
        public string Medication { get; set; }

        [Range(1, 10)]
        public int FeedingsPerDay { get; set; }

        [MaxLength(100)]
        public string FeedAmount { get; set; }

        [MaxLength(500)]
        public string FeedingInstructions { get; set; }

        [MaxLength(500)]
        public string SpecialInstructions { get; set; }

        // Navigation Properties
        [ForeignKey("OwnerId")]
        public PetOwnerModel Owner { get; set; }
        public List<PetMedicationModel> PetMedications { get; set; }
        public List<BookingModel> Bookings { get; set; }
        

        public PetModel()
        {
            PetId = Guid.NewGuid();
            PetMedications = new List<PetMedicationModel>();
            Bookings = new List<BookingModel>();
        }
    }
}