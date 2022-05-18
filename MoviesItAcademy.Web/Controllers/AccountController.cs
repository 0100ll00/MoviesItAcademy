using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesItAcademy.Domain.Enum;
using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.Service.Abstractions;
using MoviesItAcademy.Web.Models.Dtos;
using MoviesItAcademy.Web.Models.RequestModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesItAcademy.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IBookingService _bookingService;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ILogger<AccountController> logger,
                                 IBookingService bookingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _bookingService = bookingService;
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterRequestModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = new ApplicationUser()
            {
                UserName = model.Username.Substring(0, model.Username.LastIndexOf('@')),
                Email = model.Username
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            var addToRoleResult = await _userManager.AddToRoleAsync(user, Role.User.ToString());

            _logger.LogInformation($"Account created for {user.Email}.");

            if (!createResult.Succeeded)
            {
                foreach (var err in createResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View();
                }
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginRequestModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, false);
                if (signInResult.Succeeded)
                {
                    _logger.LogInformation($"User ({user.Email}) has logged in.");

                    var isSignedIn = _signInManager.IsSignedIn(User);
                    var username = User.Identity.Name;

                    var session = HttpContext.Session.GetString("BookRequestList");
                    if (session == null)
                        return RedirectToAction("Index", "Home");

                    var bookingList = JsonConvert.DeserializeObject<List<BookingDto>>(session);
                    bookingList.ForEach(x => _bookingService.BookAMovieAsync(x.MovieId, user.Id, x.BookedAt).Wait());

                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Incorrect credentials.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation($"User ({User.Identity.Name}) has logged out.");
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            if (User.Identity.IsAuthenticated)
                _logger.LogInformation($"User ({User.Identity.Name}) has attempted to access restricted resource.");
            return View();
        }
    }
}
