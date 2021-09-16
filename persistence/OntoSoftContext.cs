using Domine;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace persistence
{
    public class OntoSoftContext : IdentityDbContext<User>
    {
        public OntoSoftContext(DbContextOptions options) : base(options) {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Entity<UserAppoinments>().HasKey (ci => new {ci.IdUser, ci.AppoinmentsId});
        }

        public DbSet<User> User {get;set;}
        public DbSet<Galleries> Galleries {get;set;}
        public DbSet<Appoinments> Appoinments {get;set;}
        public DbSet<UserAppoinments> UserAppoinments {get;set;}
        public DbSet<typeProcess> typeProcess {get;set;}
        public DbSet<tooth> tooth {get;set;}
        public DbSet<Odontogram> Odontogram {get;set;}

        public DbSet<toothsOdontogram> toothsOdontogram {get;set;}
        public DbSet<typeProcessTooth> typeProcessTooth {get;set;}
        // public DbSet<typeProcessOdontogram> typeProcessOdontogram {get;set;}
        // public DbSet<userOdontogram> userOdontogram {get;set;}
        

        
    }
}
