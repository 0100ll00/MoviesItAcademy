using MoviesItAcademy.Domain.Pocos;
using System;

namespace MoviesItAcademy.Web.Models.Dtos
{
    public class BookingDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public bool IsActive { get; set; }

        public ApplicationUser User { get; set; }
        public MovieDto Movie { get; set; }

        public DateTime BookedAt { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime RunsOutAt { get; set; }
    }
}
