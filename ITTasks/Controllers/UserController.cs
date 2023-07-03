using ITTasks.Models.DTOS.Users;
using ITTasks.Models.Errors;
using ITTasks.Services.Users;
using ITTasks.Statics.Entities.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITTasks.Controllers
{
	[Authorize(Roles = RoleTypes.Admin)]
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

            if (userModel.ErrorCode != (int)ErrorCodes.NoError)
            {
                ViewBag.ErrorMessage = userModel.ErrorMessage;
                return View();
            }

            ViewBag.Successful = "با موفقیت انجام شد";
            return View();
        }

        [HttpGet("/User/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return PartialView("AllUser_Partial", users);
        }

        public async Task<IActionResult> UpdateUser()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(new UserDto
            {
                Users = users
            });
        }

        [HttpPost]
		public async Task<IActionResult> UpdateUser(UserDto user)
		{
			var users = await _userService.GetAllUsersAsync();

            var userAfterUpdate = await _userService.UpdatUserAsync(user);
            if(userAfterUpdate.ErrorCode != (int)ErrorCodes.NoError)
            {
                ViewBag.ErrorMessage = userAfterUpdate.ErrorMessage;
                return View(new UserDto
				{
					Users = users
				});

			}

			ViewBag.SuccessfulMessage = "با موفقیت انجام شد";

			return View(new UserDto
			{
				Users = users
			});
		}

        [HttpPost("User/ChangeStatus/{id}/{status}")]
        public async Task<IActionResult> ChangeStatus(Guid id, bool status)
        {
            var user = await _userService.ChangeUserStatusAsync(id, status);
            return Json(user);
        }
	}
}
