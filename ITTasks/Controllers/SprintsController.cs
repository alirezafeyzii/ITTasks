using ITTasks.Models.DTOS.Sprints;
using ITTasks.Models.Errors;
using ITTasks.Services.Sprints;
using Microsoft.AspNetCore.Mvc;

namespace ITTasks.Controllers
{
	public class SprintsController : Controller
	{
		private readonly ISprintService _sprintService;

        public SprintsController(ISprintService sprintService)
        {
			_sprintService = sprintService;

		}
        public IActionResult CreateSprint()
		
		{
			return View(new CreateSprintDto
			{
				StartDate = DateTimeOffset.Now.ToUnixTimeSeconds(),
				EndDate = DateTimeOffset.Now.ToUnixTimeSeconds(),
			});
		}

		[HttpPost]
		public async Task<IActionResult> CreateSprint(CreateSprintDto sprint)
		{
			var sprintFromRepo = await _sprintService.CreateAsync(sprint);
			if(sprintFromRepo.ErrorCode != (int)ErrorCodes.NoError)
			{
				ViewBag.ErrorMessage = sprintFromRepo.ErrorMessage;
			}

			ViewBag.Successful = "با موفقیت ثبت شد";
			return View(new CreateSprintDto
			{   
				EndDate = sprintFromRepo.EndDate,
				StartDate = sprintFromRepo.StartDate,
				Title = sprintFromRepo.Title,
			});
		}
	}
}
