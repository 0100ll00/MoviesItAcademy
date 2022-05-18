using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesItAcademy.Domain.Enum;
using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.Identity.Models;
using MoviesItAcademy.Identity.Models.RequestModels;
using MoviesItAcademy.Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace MoviesItAcademy.Identity.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserRoleController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly ILogger<UserRoleController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UserRoleController(ILogger<UserRoleController> logger,
                                  IApplicationUserService applicationUserService,
                                  UserManager<ApplicationUser> userManager,
                                  RoleManager<IdentityRole<int>> roleManager)
        {
            _logger = logger;
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> UserList(int? p)
        {
            int pizeSize = 5;
            int pageIndex = p.HasValue ? Convert.ToInt32(p) : 1;

            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRoleViewModel>();
            foreach (var user in users)
            {
                var thisViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await GetUserRoles(user)
                };

                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel.ToPagedList(pageIndex, pizeSize));
        }

        public async Task<IActionResult> Manage(int userId)
        {
            ViewBag.userId = userId;

            var applicationUser = await _userManager.FindByIdAsync(userId.ToString());
            if (applicationUser == null)
            {
                ViewBag.ErrorMessage = $"User with an id: {userId} cannot be found";
                return View("NotFound");
            }

            ViewBag.UserName = applicationUser.UserName;
            var model = new List<ManageUserRoleViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(applicationUser, role.Name))
                    userRolesViewModel.Selected = true;
                else
                    userRolesViewModel.Selected = false;

                model.Add(userRolesViewModel);
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage([FromForm] List<ManageUserRoleViewModel> model, int userId)
        {
            var applicationUser = await _userManager.FindByIdAsync(userId.ToString());
            if (applicationUser == null)
                return View();

            var roles = await _userManager.GetRolesAsync(applicationUser);

            _logger.LogInformation($"User ({User.Identity.Name}) has modified ({applicationUser.Email})'s roles, Id: {applicationUser.Id}.");

            var result = await _userManager.RemoveFromRolesAsync(applicationUser, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove selected role from the user.");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(applicationUser, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected role to the user.");
                return View(model);
            }

            return RedirectToAction("UserList");
        }

        public IActionResult AddAUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAUser(RegisterRequestModel model)
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

            _logger.LogInformation($"User ({User.Identity.Name}) has created an account for {user.Email}.");

            if (!createResult.Succeeded)
            {
                foreach (var err in createResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View();
                }
            }

            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> DeleteAUser(int id)
        {
            var applicationUser = await _userManager.FindByIdAsync(id.ToString());

            if (applicationUser == null)
                return RedirectToAction("UserList");

            return View(applicationUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAUser(ApplicationUser applicationUser)
        {
            applicationUser = await _userManager.FindByIdAsync(applicationUser.Id.ToString());

            var applicationUserRoles = await _userManager.GetRolesAsync(applicationUser);
            foreach (var role in applicationUserRoles)
                await _userManager.RemoveFromRoleAsync(applicationUser, role);

            await _applicationUserService.DeleteApplicationUserAsync(applicationUser);
            return RedirectToAction("UserList");
        }
    }
}
