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
using ITTasks.Statics;
using System.Threading.Tasks;
using ITTasks.Models.DTOS.Reports.Reporting;

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

			var users = await _userService.GetAllActiveUsersAsync();

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
				if(taskFromService.ErrorCode == (int)ErrorCodes.ConflictTask)
				{
					var builder = new TagBuilder("a");
					builder.MergeAttribute("href", "/Tasks/GetTask?id="+Guid.Parse(taskFromService.ErrorMessage));
					builder.MergeAttribute("class", "setPage");
					string a()
					{
						using (var sw = new System.IO.StringWriter())
						{
							builder.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
							return sw.ToString().Replace("><","> این<");
						}
					}
					var aaa = $"تسک با {a()}  اطلاعات قبلا قبت شده ";
					ViewBag.ErrorMessage = aaa;
					task.Sprints = allSprint;
					task.ITTaskTypes = allTaskTypes;
					task.ITTasks = allTasks;
					task.pageInfo = allTasks.FirstOrDefault().PageInfo;
					return View(task);
				}

				ViewBag.ErrorMessage = taskFromService.ErrorMessage;
				task.Sprints = allSprint;
				task.ITTaskTypes = allTaskTypes;
				task.ITTasks = allTasks;
				task.pageInfo = allTasks.FirstOrDefault().PageInfo;
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

			var users = await _userService.GetAllActiveUsersAsync();


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
				using (var r = worksheet.Cells["A1:L1"])
				{
					r.Merge = true;
					r.Style.Font.Color.SetColor(Color.White);
					r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
					r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
				}

				worksheet.Cells["A4"].Value = "نام کاربر";
				worksheet.Cells["B4"].Value = "تاریخ";
				worksheet.Cells["c4"].Value = "مدت";
				worksheet.Cells["D4"].Value = "واحد ";
				worksheet.Cells["E4"].Value = "نوع تسک";
				worksheet.Cells["F4"].Value = "اسپرینت";
				worksheet.Cells["G4"].Value = "توضیحات";
				worksheet.Cells["A4:H4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
				worksheet.Cells["A4:H4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
				worksheet.Cells["A4:H4"].Style.Font.Bold = true;
				worksheet.Cells["A4:H4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
				worksheet.Cells["A4:H4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

				
				worksheet.Cells["J4"].Value = "تعداد تسک";
				worksheet.Cells["K4"].Value = "مدت";
				worksheet.Cells["L4"].Value = "نام کاربر";
				worksheet.Cells["J4:L4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
				worksheet.Cells["J4:L4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(166, 181, 91));
				worksheet.Cells["J4:L4"].Style.Font.Bold = true;
				worksheet.Cells["J4:L4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
				worksheet.Cells["J4:L4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

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
					worksheet.Cells[row, 3].Value = task.Duration.ToStandardTime();
					worksheet.Cells[row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 4].Value = UnitsTypes.keyValues.FirstOrDefault(x =>x.Key == task.UnitId).Value;
					worksheet.Cells[row, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 5].Value = task.TaskType.Title;
					worksheet.Cells[row, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 6].Value = task.Sprint.Title;
					worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 7, row, 8].Value = task.Description;
					worksheet.Cells[row, 7, row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[row, 7, row, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[row, 7, row, 8].Style.WrapText = true;
					worksheet.Cells[row, 7, row, 8].Merge = true;


					row++;
				}

				var rowu = 5;

				foreach (var user in users)
				{
					var ids = new List<string>();
					var tasksForUser = await _taskService.GetAllTaskForUserAsync(user.Id,ids);

					var d = 0;

					foreach (var tsk4user in tasksForUser)
					{
						d += tsk4user.Duration;
					}

					worksheet.Cells[rowu, 10].Value = tasksForUser.Count;
					worksheet.Cells[rowu, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[rowu, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[rowu, 11].Value = d.ToStandardTime();
					worksheet.Cells[rowu, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[rowu, 11].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells[rowu, 12].Value = user.FullName;
					worksheet.Cells[rowu, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
					worksheet.Cells[rowu, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

					rowu++;
				}

				xlPackage.Workbook.Properties.Title = "Task List";
				xlPackage.Workbook.Properties.Author = "mehbang/AlirezaFeyzi";
				xlPackage.Workbook.Properties.Subject = "All Task";
				xlPackage.Save();
			}
			stream.Position = 0;
			return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllTask.xlsx");
			
		}
		public async Task<IActionResult> AllTask(TaskParameters param,int pageNumber = 1)
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

			return PartialView("_UpdateTaskPatial",new GetAndUpdateTaskDto
			{
				Task = new ITTaskUpdateDto
				{
					Id = task.Id,
					TaskTypeId = task.TaskType.Id,
					SprintId = task.Sprint.Id,
					Date = DateTimeOffset.Now.ToUnixTimeSeconds(),
					Duration = task.Duration,
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

		//[HttpPost("/Tasks/DeleteTaskType/{id}")]
		public async Task<IActionResult> DeleteTaskType(Guid id)
		{
			var result = await _taskTypeService.DeleteAsync(id);
			if (result)
				return RedirectToAction("AllTask");
			return BadRequest();
		}

		[HttpGet]
		public async Task<IActionResult> GetTask(Guid id)
		{
			if (id == Guid.Empty)
				return RedirectToAction("AllTask");

			var task = await _taskService.GetTaskByIdAsync(id);
			if(task == null)
				return RedirectToAction("AllTask");
			return PartialView("_GetTaskPartial",task);
		}
		
	}
}
