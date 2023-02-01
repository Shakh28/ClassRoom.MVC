using ClassRoomMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClassRoomMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger,
                             SignInManager<User> signInManager,
                             UserManager<User> userManager,
                             RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IActionResult> SignInFilter(User user)
        {
            if (user is null)
                return BadRequest("Bad Request Error");

            var isUserExist = await _userManager.FindByEmailAsync(user.Email);

            if (isUserExist == null || isUserExist!.UserName != user.UserName)
                return NotFound("Such user is not found !");

            return RedirectToAction("Menu");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            if (!await _roleManager.RoleExistsAsync(user.Role))
            {
                var role = new IdentityRole(user.Role);

                await _roleManager.CreateAsync(role);
            }
            await _userManager.CreateAsync(user, "ada1231@dD");

            await _userManager.AddToRoleAsync(user, user.Role);

            await _signInManager.SignInAsync(user, isPersistent: true);

            return RedirectToAction("Menu");
        }
        public IActionResult Index() => View();

        public IActionResult SignIn() => View();

        public IActionResult Menu() => View();
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Smth()
        {
            var users = _userManager.Users.ToList();

            return View(users);
        }
    }
}