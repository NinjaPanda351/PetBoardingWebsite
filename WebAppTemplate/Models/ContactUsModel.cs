using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models
{
    public class ContactUsModel
    {
        [Key]
        public Guid SubmissionId { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [MaxLength(20), Phone]
        public string Phone { get; set; }

        [Required, MaxLength(200)]
        public string Subject { get; set; }

        [Required, MaxLength(2000)]
        public string Message { get; set; }

        public DateTime SubmittedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public bool IsResolved { get; set; }

        public ContactUsModel()
        {
            SubmissionId = Guid.NewGuid();
            SubmittedAt = DateTime.UtcNow;
        }
    }
}