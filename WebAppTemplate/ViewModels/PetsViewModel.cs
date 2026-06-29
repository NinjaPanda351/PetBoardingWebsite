using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PawesomePalace.Models;

namespace PawesomePalace.ViewModels
{
    public class PetsIndexViewModel
    {
        public List<PetModel> Pets { get; set; } = new List<PetModel>();
    }

    public class CreatePetViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string Notes { get; set; }
    }

    public class EditPetViewModel
    {
        public Guid PetId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int? Age { get; set; }
        public string Sex { get; set; }
        public string Notes { get; set; }
    }

    public class PetDetailsViewModel
    {
        public Guid PetId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int? Age { get; set; }
        public string Sex { get; set; }
        public string Notes { get; set; }
    }
}
