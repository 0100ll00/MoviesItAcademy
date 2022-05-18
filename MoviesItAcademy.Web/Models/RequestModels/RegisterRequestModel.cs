using System.ComponentModel.DataAnnotations;

namespace MoviesItAcademy.Web.Models.RequestModels
{
    public class RegisterRequestModel
    {
        [Required]
        [EmailAddress]
        [MinLength(10)]
        [Display(Name = "Email")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
