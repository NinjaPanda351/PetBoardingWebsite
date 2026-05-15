using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models
{
    public class PetMedicationModel
    {
        [Key]
        public Guid MedicationId { get; set; }

        [Required]
        public Guid PetId { get; set; } // FK

        [Required, MaxLength(100)]
        public string MedicationName { get; set; }

        [Required, MaxLength(100)]
        public string Dosage { get; set; }

        [Required, MaxLength(100)]
        public string Frequency { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        // Navigation Properties
        [ForeignKey("PetId")]
        public PetModel Pet { get; set; }

        public PetMedicationModel()
        {
            MedicationId = Guid.NewGuid();
        }
    }
}