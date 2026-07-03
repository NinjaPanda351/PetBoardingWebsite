namespace PawesomePalace.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PawesomePalace.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PawesomePalace.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PawesomePalace.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole("Admin"));

            const string adminEmail = "pawesomeadmin@pawesomepalace.com";
            const string adminPassword = "Pp1234!";

            var admin = userManager.FindByEmail(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                userManager.Create(admin, adminPassword);
            }

            if (!userManager.IsInRole(admin.Id, "Admin"))
                userManager.AddToRole(admin.Id, "Admin");

            var defaultServices = new[]
            {
                new PawesomePalace.Models.ServiceModel { Name = "Standard Boarding",  Description = "Comfortable shared boarding in our cozy kennels.", PricePerNight = 45m, IsActive = true },
                new PawesomePalace.Models.ServiceModel { Name = "Deluxe Suite",        Description = "Private suite with extra space and premium bedding.",   PricePerNight = 75m, IsActive = true },
                new PawesomePalace.Models.ServiceModel { Name = "Daycare",             Description = "Full-day supervised play and socialization.",           PricePerNight = 35m, IsActive = true },
                new PawesomePalace.Models.ServiceModel { Name = "Grooming Add-on",    Description = "Bath, brush, and nail trim add-on service.",            PricePerNight = 30m, IsActive = true },
            };

            foreach (var svc in defaultServices)
            {
                if (!context.ServiceModels.Any(s => s.Name == svc.Name))
                    context.ServiceModels.Add(svc);
            }

            context.SaveChanges();
        }
    }
}
