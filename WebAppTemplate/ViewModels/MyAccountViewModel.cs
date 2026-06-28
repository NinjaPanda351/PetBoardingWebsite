namespace PawesomePalace.ViewModels
{
    public class MyAccountViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PetCount { get; set; }
        public int ActiveBookingCount { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public string Initials => $"{FirstName?[0]}{LastName?[0]}";
    }
}
