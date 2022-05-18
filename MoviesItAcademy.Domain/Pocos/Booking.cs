using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Domain.Pocos
{
    public class Booking
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public bool IsActive { get; set; }

        public ApplicationUser User { get; set; }
        public Movie Movie { get; set; }

        public DateTime BookedAt { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime RunsOutAt { get; set; }
    }
}
