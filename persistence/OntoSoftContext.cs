using System;
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
        }

        public DbSet<User> User {get;set;}
        public DbSet<Galleries> Galleries {get;set;}
    }
}
