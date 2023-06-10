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

namespace ITTasks.Controllers
{
	public class AuthController : Controller
	{
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

			var json = JsonConvert.SerializeObject(model);

			var data = new StringContent(json, Encoding.UTF8, "application/json");

			var loginApiUrl = "http://localhost:5000/api/Authenticate/Token";

			using var client = new HttpClient();

			var response = await client.PostAsync(loginApiUrl, data);

			if (!response.IsSuccessStatusCode)
			{
				ViewBag.ErrorMessage = "نام کاربری یا رمز اشتباه است";
				return View();
			}

			var result = await  response.Content.ReadAsStringAsync();

			GetUserByToken(result);

			var claims = new List<Claim>() {
					new Claim(ClaimTypes.NameIdentifier, Convert.ToString("1")),
						new Claim(ClaimTypes.Name, model.UserName),
						new Claim(ClaimTypes.Role, "Admin")
				};  
			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties());

			return RedirectToAction("Panel","Admin",null);
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
