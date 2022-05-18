using Mapster;
using Microsoft.Extensions.DependencyInjection;
using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.Service.Models;
using MoviesItAcademy.Web.Models.Dtos;
using MoviesItAcademy.Web.Models.RequestModels;

namespace MoviesItAcademy.Web.Infrastructure.Mappings
{
    public static class MapsterConfiguration
    {
        public static void MappingRegistration(this IServiceCollection service)
        {
            TypeAdapterConfig<MovieServiceModel, MovieDto>
                .NewConfig()
                .Map(dest => dest.Bookings, src => src.Bookings.Adapt<BookingDto>());
            TypeAdapterConfig<MovieDto, MovieServiceModel>
                .NewConfig()
                .Map(dest => dest.Bookings, src => src.Bookings.Adapt<BookingServiceModel>());

            TypeAdapterConfig<Movie, MovieServiceModel>
                .NewConfig()
                .Map(dest => dest.Uploader, src => src.UploadedBy.UserName)
                .Map(dest => dest.Bookings, src => src.Bookings.Adapt<BookingServiceModel>());
            TypeAdapterConfig<MovieServiceModel, Movie>
                .NewConfig()
                .Map(dest => dest.Bookings, src => src.Bookings.Adapt<Booking>());

            TypeAdapterConfig<BookingServiceModel, BookingDto>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<BookingServiceModel, Booking>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<MovieDto, MovieRequestModel>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<MovieRequestModel, MovieServiceModel>
                .NewConfig()
                .TwoWays();
        }
    }
}
