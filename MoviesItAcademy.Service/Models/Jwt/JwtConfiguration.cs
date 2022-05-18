namespace MoviesItAcademy.Service.Models.Jwt
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        public int ExpirationInMinutes { get; set; }
    }
}
