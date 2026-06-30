using System;
using System.Collections.Generic;

namespace PawesomePalace.ViewModels
{
    public class AdminBookingRowViewModel
    {
        public Guid BookingId { get; set; }
        public string OwnerName { get; set; }
        public string PetName { get; set; }
        public string PetSpecies { get; set; }
        public string PetBreed { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AdminCancellationRowViewModel
    {
        public Guid BookingId { get; set; }
        public string OwnerName { get; set; }
        public decimal Price { get; set; }
        public string CancellationReason { get; set; }
    }

    public class AdminReviewViewModel
    {
        public Guid BookingId { get; set; }
        public string BookingReference { get; set; }
        public string OwnerName { get; set; }
        public string ServiceType { get; set; }
        public string PetDetails { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SpecialInstructions { get; set; }
        public string AdminNotes { get; set; }
    }

    public class AdminCancellationReviewViewModel
    {
        public Guid BookingId { get; set; }
        public string BookingReference { get; set; }
        public string OwnerName { get; set; }
        public string PetDetails { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CancellationReason { get; set; }
        public decimal Price { get; set; }
        public string AdminNotes { get; set; }
    }

    public class AdminViewUserViewModel
    {
        public Guid OwnerId { get; set; }
        public string Initials { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime MemberSince { get; set; }
        public int PetCount { get; set; }
        public bool IsSuspended { get; set; }
    }

    public class AdminUserRowViewModel
    {
        public Guid OwnerId { get; set; }
        public string Initials { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int PetCount { get; set; }
        public bool IsSuspended { get; set; }
    }

    public class AdminUsersViewModel
    {
        public List<AdminUserRowViewModel> Users { get; set; } = new List<AdminUserRowViewModel>();
        public string Search { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int PendingCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int CancellationRequestCount { get; set; }
        public List<AdminBookingRowViewModel> PendingBookings { get; set; } = new List<AdminBookingRowViewModel>();
        public List<AdminCancellationRowViewModel> CancellationRequests { get; set; } = new List<AdminCancellationRowViewModel>();
    }
}
