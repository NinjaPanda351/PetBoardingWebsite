namespace PawesomePalace.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PawesomePalace.Models;
    using System.Data.Entity.Migrations;

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
        }
    }
}
