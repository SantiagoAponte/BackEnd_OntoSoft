using Domine;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
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
        public DbSet<FaceTooth> faceTooth {get;set;}
        public DbSet<typeProcessTooth> typeProcessTooth {get;set;}
        public DbSet<typeDocument> typeDocument {get;set;}
        public DbSet<BackgroundMedical> backgroundMedicals {get;set;}
        public DbSet<BackgroundOral> backgroundOrals {get;set;}
        public DbSet<OralRadiography> oralRadiography {get;set;}

        public DbSet<PatientEvolution> patientEvolution {get;set;}
        public DbSet<TreamentPlan> treamentPlan {get;set;}
        public DbSet<ClinicHistory> clinicHistories {get;set;}
        public DbSet<backgroundMedicalClinicHistory> backgroundMedicalClinicHistories {get;set;}
        public DbSet<backgroundOralClinicHistory> backgroundOralClinicHistories {get;set;}

        

        
    }
}
