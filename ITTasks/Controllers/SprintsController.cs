using ITTasks.Models.DTOS.Charts;
using ITTasks.Models.DTOS.Reports.Reporting;
using ITTasks.Models.DTOS.Sprints;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Errors;
using ITTasks.Services.Sprints;
using ITTasks.Services.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ITTasks.Controllers
{
	public class SprintsController : Controller
	{
		private readonly ISprintService _sprintService;
		private readonly ITaskService _taskService;

		public SprintsController(ISprintService sprintService,
			ITaskService taskService)
        {
			_sprintService = sprintService;
			_taskService = taskService;

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
			{   		EndDate = sprintFromRepo.EndDate,
				StartDate = sprintFromRepo.StartDate,
				Title = sprintFromRepo.Title,
			});
		}

		[Route("/sprint/GetAllSprints")]
		public async Task<IActionResult> GetAllSprints()
		{
			var a = await _taskService.GetReporting();
			return Ok(a);
		}
	}
}
