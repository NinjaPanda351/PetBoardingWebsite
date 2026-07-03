using System;
using System.ComponentModel.DataAnnotations;

namespace PawesomePalace.Models
{
    public class ServiceModel
    {
        [Key]
        public Guid ServiceId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal PricePerNight { get; set; }

        public bool IsActive { get; set; }

        public ServiceModel()
        {
            ServiceId = Guid.NewGuid();
            IsActive = true;
        }
    }
}
