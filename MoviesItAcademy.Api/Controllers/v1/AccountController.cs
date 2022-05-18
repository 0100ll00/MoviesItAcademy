using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesItAcademy.Api.Models.RequestModels;
using MoviesItAcademy.Domain.Enum;
using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.Service.Abstractions;
using System.Threading.Tasks;

namespace MoviesItAcademy.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ILogger<AccountController> logger,
                                 IApplicationUserService applicationUserService,
                                 UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _applicationUserService = applicationUserService;
            _userManager = userManager;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            var applicationUser = await _userManager.FindByNameAsync(loginRequestModel.Username);
            if (await _userManager.CheckPasswordAsync(applicationUser, loginRequestModel.Password))
            {
                _logger.LogInformation($"User ({applicationUser.Email}) has logged in.");
                return Ok($"Bearer {_applicationUserService.AuthenticateAsync(applicationUser.UserName)}");
            }
            else
                return Unauthorized();
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel registerRequestModel)
        {
            await _applicationUserService.CheckEmailUniqueAsync(registerRequestModel.Email);
            var applicationUser = new ApplicationUser()
            {
                UserName = registerRequestModel.Email.Substring(0, registerRequestModel.Email.LastIndexOf('@')),
                Email = registerRequestModel.Email
            };

            var createResult = await _userManager.CreateAsync(applicationUser, registerRequestModel.Password);
            var addToRoleResult = await _userManager.AddToRoleAsync(applicationUser, Role.User.ToString());

            _logger.LogInformation($"Account created for {applicationUser.Email}.");

            return Ok($"Bearer {_applicationUserService.AuthenticateAsync(applicationUser.UserName)}");
        }
    }
}
