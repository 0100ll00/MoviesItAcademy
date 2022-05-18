using System;
using System.Collections.Generic;

namespace MoviesItAcademy.Domain.Pocos
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public int DurationInMinutes { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }

        public int? UploadedById { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalSeats { get; set; }
        public DateTime StartsAt { get; set; }

        public ApplicationUser UploadedBy { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
