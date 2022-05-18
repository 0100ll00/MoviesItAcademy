using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoviesItAcademy.Service.Abstractions
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetUserAsync(string username);
        Task<ApplicationUser> GetUserAsync(int id);
        string AuthenticateAsync(string username);
        Task CheckEmailUniqueAsync(string email);
        Task DeleteApplicationUserAsync(ApplicationUser applicationUser);
    }
}
