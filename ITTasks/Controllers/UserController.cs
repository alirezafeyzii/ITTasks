using ITTasks.Models.DTOS.Users;
using ITTasks.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace ITTasks.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto user)
        {
            if (user == null)
            {
                ViewBag.ErrorMessage = "خطا";
                return View();
            }

            var userModel = await _userService.CreateUserAsync(user);

            if (userModel == null)
            {
                ViewBag.ErrorMessage = "خطا";
                return View();
            }

            ViewBag.Successful = "با موفقیت انجام شد";
            return View();
        }

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }
    }
}
