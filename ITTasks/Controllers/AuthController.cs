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
using MimeKit.Cryptography;
using ITTasks.Services.Mails;
using ITTasks.Services.Users;
using ITTasks.Models.DTOS.Emails;

namespace ITTasks.Controllers
{
	[AllowAnonymous]
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly IMailService _mailService;
		private readonly IUserService _userService;

		public AuthController(IAuthService authService,
			IMailService mailService,
			IUserService userService)
		{
			_authService = authService;
			_mailService = mailService;
			_userService = userService;
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

			var userFromService = await _authService.SignInAsync(model);
			if (userFromService.ErrorCode != (int)ErrorCodes.NoError)
			{
				ViewBag.ErrorMessage = userFromService.ErrorMessage;
				return View(model);
			}

			var afterSetCookie = await SetCookie(userFromService.Principal,model.RememberMe);
			if(afterSetCookie.ErrorCode != (int)ErrorCodes.NoError)
			{
				ViewBag.ErrorMessage = afterSetCookie.ErrorMessage;
				return View(model);
			}

			if (userFromService.Principal.IsInRole(RoleTypes.Admin))
				return RedirectToAction("Panel", "Admin", null);

			else if(userFromService.Principal.IsInRole(RoleTypes.User))
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

		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(CreateUserDto request)
		{
			if (request is null)
			{
				ViewBag.ErrorMessage = "پارامتر های ورودی را به درستی وارد کنید";
				return View(request);
			}

			var userFromService = await _authService.SignUpAsync(request);
			if(userFromService.ErrorCode != (int)ErrorCodes.NoError)
			{
				ViewBag.ErrorMessage = userFromService.ErrorMessage;
				return View(request);
			}

			var emailAfterSend = await _mailService.SendMailForConfirmedAsync(new Models.DTOS.Emails.MailRequestDto
			{
				ToEmail = userFromService.UserEmail,
				UserName = userFromService.UserName,
				UserFullName = userFromService.UserFullName,
				Token = userFromService.UserToken,
				Subject = "فعالسازی حساب کاربری"
			});

			if(emailAfterSend is true)
				return RedirectToAction("BeforEmailConfirmed");

			ViewBag.ErrorMessage = "مشکلی در ثبت نام رخ داده لطفا مجددا سعی کنید";
			return View(request);
		}

		public async Task<IActionResult> ConfirmEmail(string userName, string token)
		{

			if (userName is null || token is null)
				return BadRequest("error");

			var userAfterConfirmedEmail = await _authService.EmailConfirmedAsync(userName, token);
			if(userAfterConfirmedEmail.ErrorCode != (int)ErrorCodes.NoError)
			{
				return BadRequest($"error={userAfterConfirmedEmail.ErrorMessage}");
			}

			var afterSetCookie = await SetCookie(userAfterConfirmedEmail.Principal, false);
			if (afterSetCookie.ErrorCode != (int)ErrorCodes.NoError)
			{
				return BadRequest($"error={afterSetCookie.ErrorMessage}");
			}

			if (userAfterConfirmedEmail.Principal.IsInRole(RoleTypes.Admin))
				return RedirectToAction("Panel", "Admin", null);

			else if (userAfterConfirmedEmail.Principal.IsInRole(RoleTypes.User))
				return RedirectToAction("CreateTask", "Tasks", null);

			else
			{
				return BadRequest("خطا ، لطفا پارامتر هارا چک کنید و سپس دوباره سعی کنید");
			}

		}

		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgetPassword(string email)
		{
			if (email is null)
			{
				ViewBag.ErrorMessage = "وارد کردن نام کاربری یا ایمیل یا شماره تلفن الزامی است ";
				return View();
			}

			var userFromService = await _userService.GetPasswordWithEmailOrUserNameOrPhoneNumber(email);
			if(userFromService.ErrorCode != (int)ErrorCodes.NoError)
			{
				ViewBag.ErrorMessage = userFromService.ErrorMessage;
				return View();
			}

			var result = await _mailService.SendMailForgetPasswordAsync(new MailRequestDto
			{
				UserFullName = userFromService.FullName,
				ToEmail = userFromService.Email,
				Data = userFromService.Password,
				Subject = "بازیابی رمز عبور"
			});

			ViewBag.Successful = "رمز عبور با ایمیل برای شما ارسال شد";
			return View();
		}

		private async Task<AuthResponse> SetCookie(ClaimsPrincipal principal,bool rememberMe)
		{
			try
			{
				var expirationTime = DateTimeOffset.MinValue;
				var isPersistent = false;
				if (rememberMe)
				{
					expirationTime = DateTimeOffset.UtcNow.AddDays(30);
					isPersistent = true;
				}

				expirationTime = DateTimeOffset.UtcNow.AddDays(1);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
				{
					ExpiresUtc = expirationTime,
					IsPersistent = isPersistent
				});

				return new AuthResponse
				{
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError
				};
			}
			catch (Exception)
			{
				return new AuthResponse
				{
					ErrorCode = (int)ErrorCodes.ServerError,
					ErrorMessage = ErrorMessages.ServerError
				};
			}
		}

		public IActionResult BeforEmailConfirmed()
		{
			return View();
		}

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
