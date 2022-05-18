using Mapster;
using Microsoft.Extensions.DependencyInjection;
using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.Service.Models;
using MoviesItAcademy.Api.Models.Dtos;

namespace MoviesItAcademy.Api.Infrastructure.Mappings
{
    public static class MapsterConfiguration
    {
        public static void MappingRegistration(this IServiceCollection service)
        {
            TypeAdapterConfig<Movie, MovieServiceModel>
                .NewConfig()
                .Map(dest => dest.Bookings, src => src.Bookings.Adapt<BookingServiceModel>());
            TypeAdapterConfig<MovieServiceModel, Movie>
                .NewConfig()
                .Map(dest => dest.Bookings, src => src.Bookings.Adapt<Booking>());

            TypeAdapterConfig<MovieServiceModel, MovieDto>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<BookingServiceModel, BookingDto>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<BookingServiceModel, Booking>
                .NewConfig()
                .TwoWays();
        }
    }
}
