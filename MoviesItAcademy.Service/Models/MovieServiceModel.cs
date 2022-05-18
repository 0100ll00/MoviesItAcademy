using System;
using System.Collections.Generic;

namespace MoviesItAcademy.Service.Models
{
    public class MovieServiceModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public int DurationInMinutes { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalSeats { get; set; }
        public DateTime StartsAt { get; set; }

        public string Uploader { get; set; }
        public List<BookingServiceModel> Bookings { get; set; }
    }
}
