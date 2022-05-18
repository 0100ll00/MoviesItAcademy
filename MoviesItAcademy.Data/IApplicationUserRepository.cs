using MoviesItAcademy.Domain.Pocos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoviesItAcademy.Data
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser> GetNoTrackingAsync(string username);
        Task<ApplicationUser> GetNoTrackingAsync(int id);
        Task DeleteAsync(ApplicationUser applicationUser);
        Task<bool> ExistsAsync(string userName);
        Task<bool> ExistsAsync(int id);
    }
}
