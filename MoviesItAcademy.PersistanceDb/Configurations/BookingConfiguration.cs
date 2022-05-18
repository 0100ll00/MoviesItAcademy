using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesItAcademy.Domain.Pocos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.PersistanceDb.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => new { x.UserId, x.MovieId });

            builder.Property(x => x.MovieId).IsRequired();

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.IsActive).IsRequired();

            builder.Property(x => x.BookedAt).HasColumnType("datetime").IsRequired();

            builder.Property(x => x.StartsAt).HasColumnType("datetime").IsRequired();

            builder.Property(x => x.RunsOutAt).HasColumnType("datetime").IsRequired();

            builder.HasOne<Movie>(b => b.Movie)
                    .WithMany(m => m.Bookings).OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(b => b.MovieId);

            builder.HasOne<ApplicationUser>(a => a.User)
                    .WithMany(a => a.Bookings).OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(b => b.UserId);
        }
    }
}
