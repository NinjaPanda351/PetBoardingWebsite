using System;
using System.Collections.Generic;

namespace PawesomePalace.ViewModels
{
    // ── Existing ViewModels (kept intact) ────────────────────────────────────

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
        // Pet care fields
        public string VetName { get; set; }
        public string VetPhone { get; set; }
        public string MedicalNotes { get; set; }
        public string Medication { get; set; }
        public int FeedingsPerDay { get; set; }
        public string FeedAmount { get; set; }
        public string FeedingInstructions { get; set; }
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
        // Added fields
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public List<EmergencyContactViewModel> EmergencyContacts { get; set; } = new List<EmergencyContactViewModel>();
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

    // ── Emergency Contact ─────────────────────────────────────────────────────

    public class EmergencyContactViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Relationship { get; set; }
    }

    // ── Schedule ──────────────────────────────────────────────────────────────

    public class AdminScheduleItemViewModel
    {
        public Guid BookingId { get; set; }
        public string BookingReference { get; set; }
        public string PetName { get; set; }
        public string PetSpecies { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string ServiceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DropOffTime { get; set; }
        public string PickUpTime { get; set; }
        public string Status { get; set; }
        public bool MedicalAlert { get; set; }
    }

    public class AdminScheduleViewModel
    {
        public List<AdminScheduleItemViewModel> Items { get; set; } = new List<AdminScheduleItemViewModel>();
        public string DateFilter { get; set; }
    }

    // ── Bookings List ─────────────────────────────────────────────────────────

    public class AdminBookingListItemViewModel
    {
        public Guid BookingId { get; set; }
        public string BookingReference { get; set; }
        public string OwnerName { get; set; }
        public string PetName { get; set; }
        public string ServiceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }

    public class AdminBookingsListViewModel
    {
        public List<AdminBookingListItemViewModel> Items { get; set; } = new List<AdminBookingListItemViewModel>();
        public string StatusFilter { get; set; }
        public string Search { get; set; }
    }

    // ── Booking Detail ────────────────────────────────────────────────────────

    public class AdminBookingDetailViewModel
    {
        public Guid BookingId { get; set; }
        public string BookingReference { get; set; }
        public string Status { get; set; }
        public string ServiceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DropOffTime { get; set; }
        public string PickUpTime { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; }
        public string AdminNotes { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string RefundStatus { get; set; }
        // Owner
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        // Pet
        public Guid PetId { get; set; }
        public string PetName { get; set; }
        public string PetSpecies { get; set; }
        public string PetBreed { get; set; }
        public string PetSex { get; set; }
        public string VetName { get; set; }
        public string VetPhone { get; set; }
        public string MedicalNotes { get; set; }
        public string Medication { get; set; }
        public int FeedingsPerDay { get; set; }
        public string FeedAmount { get; set; }
        public string FeedingInstructions { get; set; }
        public string SpecialInstructions { get; set; }
    }

    // ── Pets List ─────────────────────────────────────────────────────────────

    public class AdminPetRowViewModel
    {
        public Guid PetId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string OwnerName { get; set; }
        public bool HasMedicalNotes { get; set; }
        public int BookingCount { get; set; }
    }

    public class AdminPetsListViewModel
    {
        public List<AdminPetRowViewModel> Pets { get; set; } = new List<AdminPetRowViewModel>();
        public string Search { get; set; }
    }

    // ── Pet Detail ────────────────────────────────────────────────────────────

    public class AdminPetDetailViewModel
    {
        public Guid PetId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Color { get; set; }
        public string SecondaryColor { get; set; }
        public string VetName { get; set; }
        public string VetPhone { get; set; }
        public string MedicalNotes { get; set; }
        public string Medication { get; set; }
        public int FeedingsPerDay { get; set; }
        public string FeedAmount { get; set; }
        public string FeedingInstructions { get; set; }
        public string SpecialInstructions { get; set; }
        // Owner
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public Guid OwnerId { get; set; }
        public List<EmergencyContactViewModel> EmergencyContacts { get; set; } = new List<EmergencyContactViewModel>();
    }

    // ── Services ──────────────────────────────────────────────────────────────

    public class ServiceViewModel
    {
        public Guid ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsActive { get; set; }
    }

    // ── Contacts ──────────────────────────────────────────────────────────────

    public class AdminContactRowViewModel
    {
        public Guid SubmissionId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public DateTime SubmittedAt { get; set; }
        public bool IsResolved { get; set; }
    }

    public class AdminContactDetailViewModel
    {
        public Guid SubmissionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime SubmittedAt { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    // ── Dashboard (updated) ───────────────────────────────────────────────────

    public class AdminDashboardViewModel
    {
        public int PendingCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int CancellationRequestCount { get; set; }
        public List<AdminBookingRowViewModel> PendingBookings { get; set; } = new List<AdminBookingRowViewModel>();
        public List<AdminCancellationRowViewModel> CancellationRequests { get; set; } = new List<AdminCancellationRowViewModel>();
        // New dashboard additions
        public int TodayCheckIns { get; set; }
        public int TodayCheckOuts { get; set; }
        public decimal MonthRevenue { get; set; }
        public List<AdminScheduleItemViewModel> CheckInsDue { get; set; } = new List<AdminScheduleItemViewModel>();
    }
}
