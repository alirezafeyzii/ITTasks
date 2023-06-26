using ITTasks.Models.DTOS.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS.Users;
using System.IdentityModel.Tokens.Jwt;
using ITTasks.Services.Auth;
using ITTasks.Models.Errors;
using ITTasks.Statics.Entities.Roles;
using Azure.Core;

namespace ITTasks.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;

		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDto model)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.ErrorMessage = "نام کاربری یا رمز را وارد نکردید";
				return View(model);
			}

			var userFromservice = await _authService.SignInAsync(model);
			if (userFromservice.ErrorCode != (int)ErrorCodes.NoError)
			{
				ViewBag.ErrorMessage = userFromservice.ErrorMessage;
				return View(model);
			}

			var expirationTime = DateTimeOffset.MinValue;

			if (model.RememberMe)
				expirationTime = DateTimeOffset.UtcNow.AddDays(30);

			expirationTime = DateTimeOffset.UtcNow.AddDays(1);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userFromservice.Principal, new AuthenticationProperties
			{
				ExpiresUtc = expirationTime
			});

			if (userFromservice.Principal.IsInRole(RoleTypes.Admin))
				return RedirectToAction("Panel", "Admin", null);

			else if(userFromservice.Principal.IsInRole(RoleTypes.User))
				return RedirectToAction("CreateTask", "Tasks", null);

			else
			{
				ViewBag.ErrorMessage = "خطا ، لطفا پارامتر هارا چک کنید و سپس دوباره سعی کنید";
				return View(model);
			}
			//if(model.UserName == "admin" && model.Password == "admin")
			//{
			//	var claims = new List<Claim>() {
			//		new Claim(ClaimTypes.NameIdentifier, Convert.ToString("1")),
			//			new Claim(ClaimTypes.Name, "علیرضا فیضی"),
			//			new Claim(ClaimTypes.Role, "Admin")
			//	};
			//	var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			//	var principal = new ClaimsPrincipal(identity);
			//	await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties());

			//	//if (User.IsInRole("Admin"))
			//	//{
			//	//	return RedirectToAction("Panel", "Admin", null);
			//	//}

			//	return RedirectToAction("CreateTask", "Tasks",null);
			//}



			//ViewBag.ErrorMessage = "invalid...";

			//return View();
		}

		private void GetUserByToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var readToken = tokenHandler.ReadJwtToken(token);

			var userName = readToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
			var userRoles = readToken.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
			//var userId = readToken.Claims.FirstOrDefault(c => c.);
		}
	}
}
