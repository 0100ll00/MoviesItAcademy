using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesItAcademy.Domain.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoviesItAcademy.PersistanceDb.Context
{
    public class MoviesItAcademyContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public MoviesItAcademyContext(DbContextOptions<MoviesItAcademyContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoviesItAcademyContext).Assembly);
        }
    }
}
