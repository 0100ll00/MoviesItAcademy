using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesItAcademy.Domain.Pocos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.PersistanceDb.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsUnicode(false).IsRequired().HasMaxLength(50);

            builder.Property(x => x.DurationInMinutes).HasColumnType("integer").IsRequired();

            builder.Property(x => x.Year).HasColumnType("integer").IsRequired();

            builder.Property(x => x.Country).IsUnicode(false).IsRequired().HasMaxLength(50);

            builder.Property(x => x.IsActive);

            builder.Property(x => x.IsDeleted);

            builder.Property(x => x.TotalSeats).HasColumnType("integer").IsRequired();

            builder.Property(x => x.StartsAt).HasColumnType("datetime").IsRequired();

            builder.Property(x => x.UploadedById).IsRequired(false);

            builder.HasOne(x => x.UploadedBy)
                .WithMany(x => x.Movies)
                .HasForeignKey(x => x.UploadedById)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}
