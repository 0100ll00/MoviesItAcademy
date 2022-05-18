using MoviesItAcademy.Domain.Pocos;
using System;

namespace MoviesItAcademy.Service.Models
{
    public class BookingServiceModel
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public bool IsActive { get; set; }

        public ApplicationUser User { get; set; }
        public MovieServiceModel Movie { get; set; }

        public DateTime BookedAt { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime RunsOutAt { get; set; }
    }
}
