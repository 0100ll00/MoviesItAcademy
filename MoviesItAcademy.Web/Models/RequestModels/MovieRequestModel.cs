using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesItAcademy.Web.Models.RequestModels
{
    public class MovieRequestModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Title { get; set; }
        [Url]
        [Required]
        [DataType(DataType.ImageUrl)]
        public string ThumbnailUrl { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int DurationInMinutes { get; set; }
        [Required]
        [Range(1888, 2100)]
        public int Year { get; set; }
        [Required]
        public string Country { get; set; }
        [ScaffoldColumn(false)]
        public bool IsActive { get; set; }
        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }
        [Required]
        public int TotalSeats { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartsAt { get; set; }
    }
}
