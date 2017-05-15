using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JobBoardMVC.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Languages { get; set; }
        public string Experience { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class JobBoardMvcContext : IdentityDbContext<User>
    {
        public JobBoardMvcContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static JobBoardMvcContext Create()
        {
            return new JobBoardMvcContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim").Property(p => p.Id).HasColumnName("UserClaimId");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles").Property(p => p.Id).HasColumnName("RoleId");


        }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Company> Companies { get; set; }


    }
}