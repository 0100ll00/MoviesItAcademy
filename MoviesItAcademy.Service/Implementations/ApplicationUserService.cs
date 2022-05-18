using MoviesItAcademy.Data;
using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.Service.Abstractions;
using MoviesItAcademy.Service.Exceptions;
using System.Threading.Tasks;

namespace MoviesItAcademy.Service.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IJwtService _jwtService;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserService(IJwtService jwtService, IApplicationUserRepository userRepository)
        {
            _jwtService = jwtService;
            _applicationUserRepository = userRepository;
        }

        public async Task<ApplicationUser> GetUserAsync(string username)
        {
            var user = await _applicationUserRepository.GetNoTrackingAsync(username);
            return user;
        }

        public async Task<ApplicationUser> GetUserAsync(int id)
        {
            var user = await _applicationUserRepository.GetNoTrackingAsync(id);
            return user;
        }

        public async Task DeleteApplicationUserAsync(ApplicationUser applicationUser)
        {
            var doesUserExist = await _applicationUserRepository.ExistsAsync(applicationUser.UserName);
            if (doesUserExist)
                throw new ApplicationUsernameNotFoundException(applicationUser.UserName);

            await _applicationUserRepository.DeleteAsync(applicationUser);
        }

        public string AuthenticateAsync(string username)
        {
            return _jwtService.GenerateSecurityToken(username);
        }

        public async Task CheckEmailUniqueAsync(string email)
        {
            if (await _applicationUserRepository.ExistsAsync(email))
                throw new EmailAlreadyRegisteredException(email);
        }
    }
}
