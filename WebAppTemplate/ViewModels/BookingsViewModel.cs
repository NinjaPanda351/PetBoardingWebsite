using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PawesomePalace.ViewModels
{
    // Represents one booking row in the list
    public class BookingListItemViewModel
    {
        public Guid BookingId { get; set; }
        public string PetName { get; set; }
        public string PetSpecies { get; set; }
        public string ServiceType { get; set; }
        public string BookingReference { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }

    public class BookingsIndexViewModel
    {
        public List<BookingListItemViewModel> Bookings { get; set; } = new List<BookingListItemViewModel>();
        public string ActiveFilter { get; set; }
    }

    public class BookingDetailViewModel
    {
        public Guid BookingId { get; set; }
        public string BookingReference { get; set; }
        public string ServiceType { get; set; }
        public string Status { get; set; }
        public string PetName { get; set; }
        public string PetSex { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DropOffTime { get; set; }
        public string PickUpTime { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string RefundStatus { get; set; }
    }

    public class CancelBookingViewModel
    {
        public Guid BookingId { get; set; }
        public string BookingReference { get; set; }

        [Required]
        public string CancellationReason { get; set; }
    }

    public class EditBookingViewModel
    {
        public Guid BookingId { get; set; }
        public string BookingReference { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string DropOffTime { get; set; }
        public string PickUpTime { get; set; }
        public string SpecialInstructions { get; set; }
    }

    public class CreateBookingViewModel
    {
        public List<SelectListItem> PetOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ServiceOptions { get; set; } = new List<SelectListItem>();

        [Required]
        public string PetId { get; set; }

        [Required]
        public string ServiceType { get; set; } = "Standard Boarding";

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string DropOffTime { get; set; } = "09:00";
        public string PickUpTime { get; set; } = "17:00";

        public string SpecialInstructions { get; set; }
    }
}