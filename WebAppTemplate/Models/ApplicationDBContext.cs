using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PawesomePalace.Models
{
     public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<EmergencyContactModel> EmergencyContactModels { get; set; }
        public DbSet<PetOwnerModel> PetOwnerModels { get; set; }
        public DbSet<EmployeeModel> EmployeeModels { get; set; }
        public DbSet<PetModel> PetModels { get; set; }
        public DbSet<PetMedicationModel> PetMedicationModels { get; set; }
        public DbSet<BookingModel> BookingModels { get; set; }
        public DbSet<BookingEventModel> BookingEventModels { get; set; }
        public DbSet<ContactUsModel> ContactUsModels { get; set; }
        public DbSet<ServiceModel> ServiceModels { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}