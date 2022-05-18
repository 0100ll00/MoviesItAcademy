using System;
using System.Collections.Generic;

namespace MoviesItAcademy.Web.Models.Dtos
{
    public class MovieDto
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
        public List<BookingDto> Bookings { get; set; }
    }
}
