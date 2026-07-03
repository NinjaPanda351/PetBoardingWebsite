using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PawesomePalace.ViewModels
{
    public class PetListItemViewModel
    {
        public Guid PetId { get; set; }
        public string Species { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class PetsIndexViewModel
    {
        public List<PetListItemViewModel> Pets { get; set; } = new List<PetListItemViewModel>();
    }

    public abstract class PetFormViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string Sex { get; set; }
        public string Color { get; set; }
        public string SecondaryColor { get; set; }
        [Required]
        public string VetName { get; set; }
        [Required]
        public string VetPhone { get; set; }
        public string MedicalNotes { get; set; }
        public string Medication { get; set; }
        [Required, Range(1, 10)]
        public int FeedingsPerDay { get; set; } = 1;
        [Required]
        public string FeedAmount { get; set; }
        public string FeedingInstructions { get; set; }
        public string SpecialInstructions { get; set; }
    }

    public class CreatePetViewModel : PetFormViewModel
    {
    }

    public class EditPetViewModel : PetFormViewModel
    {
        public Guid PetId { get; set; }
    }

    public class PetDetailsViewModel
    {
        public Guid PetId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Sex { get; set; }
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
    }
}
