using ITTasks.DataLayer.Entities;
using ITTasks.Infrastructure.Extentions;
using ITTasks.Models.DTOS;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Errors;
using ITTasks.Models.Parameters;
using ITTasks.Services.Tasks;
using ITTasks.Services.Tasks.TasksType;
using ITTasks.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using ITTasks.Services.Sprints;
using ITTasks.Models.DTOS.Tasks.TasksType;
using ITTasks.Models.DTOS.Tasks.GetAndUpdate;

namespace ITTasks.Controllers
{
	public class TasksController : Controller
	{
		private readonly IUserService _userService;
		private readonly ITaskService _taskService;
		private readonly ITaskTypeService _taskTypeService;
		private readonly ISprintService _sprintService;

		public TasksController(IUserService userService,
			ITaskService taskService,
			ITaskTypeService taskTypeService,
			ISprintService sprintService)
		{
			_userService = userService;
			_taskService = taskService;
			_taskTypeService = taskTypeService;
			_sprintService = sprintService;
		}

		public async Task<IActionResult> CreateTask(int pageNumber = 1)
		{
			var param = new TaskParameters { PageNumber = pageNumber };

			var users = await _userService.GetAllUsersAsync();

			var allTaskType = await _taskTypeService.GetAllAsync();

			var allSprint = await _sprintService.GetAllAsync();

			var allTasks = await _taskService.GetAllTasksAsync(param);
			if (allTasks.Count == 0)
			{
				var viewModel = new ITTaskCreateDto
				{
					Date = DateTimeOffset.Now.ToUnixTimeSeconds(),
					Users = users,
					ITTasks = allTasks,
					ITTaskTypes = allTaskType,
					Sprints = allSprint
				};
				return View(viewModel);
			}

			var model = new ITTaskCreateDto
			{
				Date = DateTimeOffset.Now.ToUnixTimeSeconds(),
				Users = users,
				ITTasks = allTasks,
				ITTaskTypes = allTaskType,
				pageInfo = allTasks.FirstOrDefault().PageInfo,
				Sprints = allSprint
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CreateTask(ITTaskCreateDto task)
		{
			var usersGroup = task.Users = await _userService.GetAllUsersAsync();

			var allTaskTypes = task.ITTaskTypes = await _taskTypeService.GetAllAsync();

			var allSprint = await _sprintService.GetAllAsync();

			var taskFromService = await _taskService.CreateTaskAsync(task);

			var allTasks = await _taskService.GetAllTasksAsync(new Models.Parameters.TaskParameters());

			if (taskFromService.ErrorCode != (int)ErrorCodes.NoError)
			{
				ViewBag.ErrorMessage = taskFromService.ErrorMessage;
				task.Sprints = allSprint;
				task.ITTaskTypes = allTaskTypes;
				task.ITTasks = allTasks;
				return View(task);
			}

			ViewBag.SuccessfulMessage = "با موفقیت انجام شد";

			//return View(new ITTaskCreateDto
			//         {
			//             UserId = taskFromService.UserId.ToString(),
			//             TaskDateTime = taskFromService.Date,
			//             Description = taskFromService.Description,
			//             FullName = taskFromService.User.FullName,   
			//             HourAmount = taskFromService.HourAmount,
			//             Users = usersGroup,
			//             ITTasks = allTasks,
			//             PersianDate = DateTimeExtention.ToPersianWithOutTime(taskFromService.Date),
			//             ITTaskTypes = allTaskTypes,
			//             Title = taskFromService.TaskType.Title,
			//             ErrorMessage = taskFromService.TaskType.ErrorMessage,
			//             ErrorCode = taskFromService.ErrorCode,
			//	UserAdded = true,
			//             pageInfo = allTasks.FirstOrDefault().PageInfo,
			//	Sprints = allSprint
			//});

			return RedirectToAction("CreateTask");
		}


		[HttpPost("/Tasks/DeleteTask/{id}")]
		public async Task<IActionResult> DeleteTask([FromRoute] string id)
		{
			var taskFromService = await _taskService.DeleteTaskAsync(Guid.Parse(id));

			return Json(new BaseDTO
			{
				ErrorCode = 1,
				ErrorMessage = "Error"
			});
		}

		public async Task<IActionResult> ExcelAllTask()
		{
			var tasks = await _taskService.GetAllWithOutPaging();



			var stream = new MemoryStream();
			using (var xlPackage = new ExcelPackage(stream))
			{
				var worksheet = xlPackage.Workbook.Worksheets.Add("AllTask");
				var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
				namedStyle.Style.Font.UnderLine = true;
				namedStyle.Style.Font.Color.SetColor(Color.Blue);
				const int startRow = 5;
				var row = startRow;
				worksheet.View.RightToLeft = true;

				worksheet.Cells["A1"].Value = "اکسل تسک های خارج از اسپرینت IT";
				using (var r = worksheet.Cells["A1:G1"])
				{
					r.Merge = true;
					r.Style.Font.Color.SetColor(Color.White);
					r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
					r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
				}

				worksheet.Cells["A4"].Value = "نام کاربر";
				worksheet.Cells["B4"].Value = "تاریخ";
				worksheet.Cells["c4"].Value = "مقدار ساعت";
				worksheet.Cells["D4"].Value = "نوع تسک";
				worksheet.Cells["E4"].Value = "اسپرینت";
				worksheet.Cells["F4"].Value = "توضیحات";
				worksheet.Cells["A4:G4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
				worksheet.Cells["A4:G4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
				worksheet.Cells["A4:G4"].Style.Font.Bold = true;
				worksheet.Cells["A4:G4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
				worksheet.Cells["A4:G4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

				row = 5;
				foreach (var task in tasks)
				{
					worksheet.DefaultColWidth = 20;
					worksheet.Cells[row, 1].Value = task.User.FullName;
					worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 2].Value = DateTimeExtention.ToPersianWithOutTime(task.Date);
					worksheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 3].Value = task.HourAmount;
					worksheet.Cells[row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 4].Value = task.TaskType.Title;
					worksheet.Cells[row, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 5].Value = task.Sprint.Title;
					worksheet.Cells[row, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 6, row, 7].Value = task.Description;
					worksheet.Cells[row, 6, row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 6, row, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 6, row, 7].Style.WrapText = true;
					worksheet.Cells[row, 6, row, 7].Merge = true;

					row++;
				}
				xlPackage.Workbook.Properties.Title = "Task List";
				xlPackage.Workbook.Properties.Author = "mehbang/AlirezaFeyzi";
				xlPackage.Workbook.Properties.Subject = "All Task";
				xlPackage.Save();
			}
			stream.Position = 0;
			return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllTask.xlsx");
		}
		public async Task<IActionResult> AllTask(int pageNumber = 1)
		{
			var allTasks = await _taskService.GetAllTasksAsync(new TaskParameters { PageNumber = pageNumber });

			return View(allTasks);
		}
		public async Task<IActionResult> CreateTaskType()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateTaskType(ITTaskTypeCreateDto taskType)
		{
			var taskTYpeFromService = await _taskTypeService.CreateAsync(taskType);
			if (taskTYpeFromService.ErrorCode != (int)ErrorCodes.NoError)
			{
				ViewBag.ErrorMessage = taskTYpeFromService.ErrorMessage;
				return View(taskType);
			}

			ViewBag.SuccessfulMessage = "با موفقیت انجام شد";
			return View(taskType);
		}

		public async Task<IActionResult> UpdateTask(Guid id)
		{
			if (id == Guid.Empty)
			{
				ViewBag.ErrorMessage = "شناسه تسک نمی تواند خالی باشد";
				return View();
			}

			var task = await _taskService.GetTaskByIdAsync(id);
			if (task.ErrorCode != (int)ErrorCodes.NoError)
			{
				ViewBag.ErrorMessage = task.ErrorMessage;
				return View();
			}

			var allTaskType = await _taskTypeService.GetAllAsync();

			var allUser = await _userService.GetAllUsersAsync();

			var allSprint = await _sprintService.GetAllAsync();

			return View(new GetAndUpdateTaskDto
			{
				Task = new ITTaskUpdateDto
				{
					Id = task.Id,
					TaskTypeId = task.TaskType.Id,
					SprintId = task.Sprint.Id,
					Date = DateTimeOffset.Now.ToUnixTimeSeconds(),
					HourAmount = task.HourAmount,
					Description = task.Description,
				},
				TaskTypes = allTaskType,
				Users = allUser,
				Sprints = allSprint,
				User = task.User,
				TaskType = task.TaskType,
				Sprint = task.Sprint
			});
		}

		[HttpPost]
		public async Task<IActionResult> UpdateTask(ITTaskUpdateDto task)
		{
			if (task == null)
				return View();

			var taskAfterModified = await _taskService.UpdateTaskAsync(task);
			if (taskAfterModified.ErrorCode != (int)ErrorCodes.NoError)
			{
				return View();
			}

			return RedirectToAction("CreateTask");
		}

	}
}
