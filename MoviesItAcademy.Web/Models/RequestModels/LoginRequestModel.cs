using System.ComponentModel.DataAnnotations;

namespace MoviesItAcademy.Web.Models.RequestModels
{
    public class LoginRequestModel
    {
        [Required]
        [Display(Name = "Username")]
        [MinLength(4)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberLogin { get; set; }
    }
}
