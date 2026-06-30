using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
}