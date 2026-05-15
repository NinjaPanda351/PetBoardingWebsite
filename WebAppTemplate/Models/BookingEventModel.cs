using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models
{
    public class BookingEventModel
    {
        [Key]
        public Guid EventId { get; set; }
        [Required]
        public Guid BookingId { get; set; } // FK

        [Required]
        public Guid EmployeeId { get; set; } // FK

        [Required, MaxLength(100)]
        public string EventType { get; set; }

        [Required]
        public DateTime EventTime { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        // Navigation Properties
        [ForeignKey("BookingId")]
        public BookingModel Booking { get; set; }

        [ForeignKey("EmployeeId")]
        public EmployeeModel Employee { get; set; }

        public BookingEventModel() 
        {
            EventId = Guid.NewGuid();
        }

    }
}