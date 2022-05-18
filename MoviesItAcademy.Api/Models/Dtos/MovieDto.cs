using System;

namespace MoviesItAcademy.Api.Models.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public int DurationInMinutes { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }

        public int TotalSeats { get; set; }
        public DateTime StartsAt { get; set; }
    }
}
