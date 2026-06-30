using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PawesomePalace.Models
{
    public class BookingModel
    {
        [Key]
        public Guid BookingId { get; set; }
        
        [Required]
        public Guid PetId { get; set; } // FK

        [Required]
        public DateTime BookingStartTime { get; set; }

        [Required]
        public DateTime BookingEndTime { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        [MaxLength(500)]
        public string CancellationReason { get; set; }

        [MaxLength(2000)]
        public string AdminNotes { get; set; }

        // null = not yet reviewed, "Approved", "Denied"
        [MaxLength(20)]
        public string RefundStatus { get; set; }

        // Navigation Properties
        [ForeignKey("PetId")]
        public PetModel Pet { get; set; }

        [Required]
        public string ServiceType { get; set; }
        public decimal Price { get; set; }

        [Required]
        public string BookingReference { get; set; }



        public BookingModel()
        {
            BookingId = Guid.NewGuid();
        }
    }
}