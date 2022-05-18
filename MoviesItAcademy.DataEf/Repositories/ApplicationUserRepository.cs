using Microsoft.EntityFrameworkCore;
using MoviesItAcademy.Data;
using MoviesItAcademy.Domain.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MoviesItAcademy.DataEf.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private IBaseRepository<ApplicationUser> _baseRepository;

        public ApplicationUserRepository(IBaseRepository<ApplicationUser> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _baseRepository.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _baseRepository.AnyAsync(x => x.Id == id);
        }
        
        public async Task DeleteAsync(ApplicationUser applicationUser)
        {
            var moviesToUpdate = await _baseRepository.Table
                .Include(x => x.Movies)
                .SelectMany(x => x.Movies.Where(y => y.UploadedById == applicationUser.Id))
                .ToListAsync();

            moviesToUpdate.ForEach(x => x.UploadedById = null);
            await _baseRepository.UpdateAsync(applicationUser);

            var applicationUserFull = await _baseRepository.Table
                .Where(x => x.Id == applicationUser.Id)
                .Include(x => x.Bookings)
                .SingleOrDefaultAsync(x => x.Id == applicationUser.Id);

            await _baseRepository.RemoveAsync(applicationUserFull);
        }
        
        public async Task<ApplicationUser> GetNoTrackingAsync(string username)
        {
            return await _baseRepository.TableNoTracking.SingleOrDefaultAsync(x => x.UserName == username);
        }
        
        public async Task<ApplicationUser> GetNoTrackingAsync(int id)
        {
            return await _baseRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
