using ITTasks.Models.DTOS.Reports.Reporting;
using ITTasks.Services.Sprints;
using ITTasks.Services.Tasks.TasksType;
using ITTasks.Services.Tasks;
using ITTasks.Services.Users;
using Microsoft.AspNetCore.Mvc;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Parameters;
using ITTasks.Infrastructure.Extentions;
using ITTasks.Statics;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using ChartDirector;
using ITTasks.DataLayer.Entities;

namespace ITTasks.Controllers
{
	public class ReportsController : Controller
	{
		private readonly IUserService _userService;
		private readonly ITaskService _taskService;
		private readonly ITaskTypeService _taskTypeService;
		private readonly ISprintService _sprintService;
		private readonly IWebHostEnvironment _hostEnv;

        public ReportsController(IUserService userService,
			ITaskService taskService,
			ITaskTypeService taskTypeService,
			ISprintService sprintService,
			IWebHostEnvironment hostEnv)
        {
			_userService = userService;
			_taskService = taskService;
			_taskTypeService = taskTypeService;
			_sprintService = sprintService;
			_hostEnv = hostEnv;
		}

        public async Task<IActionResult> Reporting()
		{
			var param = new TaskParameters();

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
					Sprints = allSprint.Any() ? allSprint : new List<Models.DTOS.Sprints.SprintDto>(),
					FromDate = DateTimeOffset.Now.ToUnixTimeSeconds(),
					ToDate = DateTimeOffset.Now.ToUnixTimeSeconds(),
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
				Sprints = allSprint,
				FromDate = DateTimeOffset.Now.ToUnixTimeSeconds(),
				ToDate = DateTimeOffset.Now.ToUnixTimeSeconds(),
			};
			return View(model);
		}

		[HttpPost("/Reports/Reports_Partial")]
		public async Task<IActionResult> Reports_Partial([FromBody]ReportingSearchDto request)
		{
			var tasks = await _taskService.GetTasksForReportingAsync(request);

			return PartialView("Reports_Partial",tasks);
		}

		public async Task<string> ExcelTasks(ReportingSearchDto request)
		{
			var tasks = await _taskService.GetTasksForReportingAsync(request);
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
					worksheet.Cells[row, 4].Value = UnitsTypes.keyValues.FirstOrDefault(x => x.Key == task.UnitId).Value;
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
					//var tasksForUser = await _taskService.GetAllTaskForUserAsync(user.Id, request.SprintIds);
					var tasksForUser = tasks.Where(x => x.User.Id == user.Id).ToList();
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
			var file = File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tasks.xlsx");

			string path = Path.Combine(_hostEnv.WebRootPath, "ExcelFiles");
			string uniquFileName = DateTime.Now.ToString("MM-dd mm-ss-ff") + file.FileDownloadName;
			string filePath = Path.Combine(path, uniquFileName);
			var standardPath = filePath.Replace("\\", "/");
			using (var strm = new FileStream(standardPath, FileMode.Create))
			{
				file.FileStream.CopyTo(strm);
			}
			var url = $"{Urls.BaseUrl}/ExcelFiles/" + uniquFileName;
			return url;
		}
		[HttpPost("/Repots/GetTasksExcelUrl")]
		public async Task< string> GetTasksExcelUrl([FromBody]ReportingSearchDto request)
		{
			var url = await ExcelTasks(request);
			return url;
		}


        public ActionResult Index()
        {
            createChart(ViewBag.Viewer = new RazorChartViewer(HttpContext, "chart1"));
            return View();
        }
        private void createChart(RazorChartViewer viewer)
        {
            double[] data = { 85, 156 };

            string[] labels = { "اول اردیبهشت", "اخر اردیبهشت" };
            XYChart c = new XYChart(250, 250);
            c.setPlotArea(30, 20, 200, 200);
            c.addBarLayer(data);
            c.xAxis().setLabels(labels);
            viewer.Image = c.makeWebImage(Chart.JPG);
            viewer.ImageMap = c.getHTMLImageMap("", "", "title='{xLabel}: {value} GBytes'");
        }
    }
}
