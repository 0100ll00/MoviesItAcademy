using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesItAcademy.Domain.Enum;
using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.PersistanceDb.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.PersistanceDb.Seed
{
    public class MoviesItAcademySeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var database = scope.ServiceProvider.GetRequiredService<MoviesItAcademyContext>();

            Migrate(database);
            SeedEverything(database, userManager, roleManager);
        }

        private static void SeedEverything(MoviesItAcademyContext context,
                                           UserManager<ApplicationUser> userManager,
                                           RoleManager<IdentityRole<int>> roleManager)
        {
            var seeded = false;

            SeedRoles(context, roleManager, ref seeded);
            SeedUser(context, userManager, ref seeded);
            SeedMovie(context, ref seeded);

            if (seeded)
                context.SaveChanges();
        }
        private static void Migrate(MoviesItAcademyContext context)
        {
            context.Database.Migrate();
        }
        private static void SeedRoles(MoviesItAcademyContext context, RoleManager<IdentityRole<int>> roleManager, ref bool seeded)
        {
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                var roleToSeed = new IdentityRole<int>(role.ToString());
                if (context.Roles.ContainsAsync(roleToSeed).Result)
                    continue;
                roleManager.CreateAsync(roleToSeed).Wait();
                seeded = true;
            }
        }
        private static void SeedUser(MoviesItAcademyContext context, UserManager<ApplicationUser> userManager, ref bool seeded)
        {
            var adminUser = new ApplicationUser()
            {
                UserName = "sysadmin",
                Email = "sysadmin@moviesitacademy.com",
            };

            if (!context.Users.AnyAsync(x => x.UserName == adminUser.UserName).Result)
            {
                userManager.CreateAsync(adminUser, "Sysadmin16!").Wait();
                userManager.AddToRoleAsync(adminUser, Role.Administrator.ToString()).Wait();
                seeded = true;
            }
        }
        private static void SeedMovie(MoviesItAcademyContext context, ref bool seeded)
        {
            var movieList = new List<Movie>()
            {
                new Movie()
                {
                    Title = "The Joker",
                    ThumbnailUrl = "https://m.media-amazon.com/images/I/71HBOO7tY5L._AC_SL1500_.jpg",
                    DurationInMinutes = 122,
                    Year = 2019,
                    Country = "USA",

                    IsActive = true,
                    IsDeleted = false,
                    TotalSeats = 30,
                    StartsAt = DateTime.Parse("2019-03-05"),

                    UploadedById = 1
                },
                new Movie()
                {
                    Title = "The Avengers",
                    ThumbnailUrl = "https://media.comicbook.com/2018/03/avengers-infinity-war-poster-1093756.jpeg",
                    DurationInMinutes = 149,
                    Year = 2018,
                    Country = "USA",

                    IsActive = true,
                    IsDeleted = false,
                    TotalSeats = 30,
                    StartsAt = DateTime.Parse("2022-04-20"),

                    UploadedById = 1
                },
                new Movie()
                {
                    Title = "V for Vendetta",
                    ThumbnailUrl = "https://i.pinimg.com/originals/18/0d/84/180d8456364904a261bb874c23aed0d0.png",
                    DurationInMinutes = 132,
                    Year = 2005,
                    Country = "USA",

                    IsActive = true,
                    IsDeleted = false,
                    TotalSeats = 30,
                    StartsAt = DateTime.Parse("2022-04-22"),

                    UploadedById = 1
                },
                new Movie()
                {
                    Title = "Dune",
                    ThumbnailUrl = "https://popcornreviewss.com/wp-content/uploads/2021/10/Dune-2021-English-Adventure-SciFi-Movie-Review-scaled.jpg",
                    DurationInMinutes = 155,
                    Year = 2021,
                    Country = "USA",

                    IsActive = true,
                    IsDeleted = false,
                    TotalSeats = 30,
                    StartsAt = DateTime.Parse("2022-04-19"),

                    UploadedById = 1
                },
                new Movie()
                {
                    Title = "The Matrix",
                    ThumbnailUrl = "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_FMjpg_UX1000_.jpg",
                    DurationInMinutes = 136,
                    Year = 1999,
                    Country = "USA",

                    IsActive = true,
                    IsDeleted = false,
                    TotalSeats = 30,
                    StartsAt = DateTime.Parse("2022-04-24"),

                    UploadedById = 1
                },
                new Movie()
                {
                    Title = "John Wick",
                    ThumbnailUrl = "https://m.media-amazon.com/images/M/MV5BMTU2NjA1ODgzMF5BMl5BanBnXkFtZTgwMTM2MTI4MjE@._V1_.jpg",
                    DurationInMinutes = 101,
                    Year = 2014,
                    Country = "USA",

                    IsActive = true,
                    IsDeleted = false,
                    TotalSeats = 30,
                    StartsAt = DateTime.Parse("2022-04-25"),

                    UploadedById = 1
                },
                new Movie()
                {
                    Title = "Batman Begins",
                    ThumbnailUrl = "https://m.media-amazon.com/images/M/MV5BOTY4YjI2N2MtYmFlMC00ZjcyLTg3YjEtMDQyM2ZjYzQ5YWFkXkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_.jpg",
                    DurationInMinutes = 140,
                    Year = 2005,
                    Country = "USA",

                    IsActive = true,
                    IsDeleted = false,
                    TotalSeats = 30,
                    StartsAt = DateTime.Parse("2022-04-27"),

                    UploadedById = 1
                }
            };

            foreach (var movie in movieList)
            {
                if (!context.Movies.AnyAsync(x => x.Title == movie.Title).Result)
                {
                    context.Movies.Add(movie);
                    seeded = true;
                }
            }
        }
    }
}
