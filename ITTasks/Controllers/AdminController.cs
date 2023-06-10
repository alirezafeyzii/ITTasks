using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace ITTasks.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		[HttpGet("/Admin/Panel")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Panel()

		{
			return View();
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit()
		{
			var token = Request.Headers.Cookie;
			return View();
		}
	}
}
