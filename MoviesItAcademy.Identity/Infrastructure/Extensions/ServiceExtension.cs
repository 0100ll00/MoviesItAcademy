using Microsoft.Extensions.DependencyInjection;
using MoviesItAcademy.Data;
using MoviesItAcademy.DataEf;
using MoviesItAcademy.DataEf.Repositories;
using MoviesItAcademy.Service.Abstractions;
using MoviesItAcademy.Service.Implementations;

namespace MoviesItAcademy.Identity.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServicesAndRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IBookingService, BookingService>();

            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }
    }
}
