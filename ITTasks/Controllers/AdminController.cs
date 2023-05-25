using Microsoft.AspNetCore.Mvc;

namespace ITTasks.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Panel()
		{
			return View();
		}
	}
}
