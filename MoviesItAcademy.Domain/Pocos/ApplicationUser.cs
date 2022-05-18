using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MoviesItAcademy.Domain.Pocos
{
    public class ApplicationUser : IdentityUser<int>
    {
        public List<Movie> Movies { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
