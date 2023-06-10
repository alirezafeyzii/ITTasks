using Azure.Core;
using ITTasks.DataLayer;
using ITTasks.DataLayer.Entities;
using ITTasks.Infrastructure.Helper;
using ITTasks.Infrastructure.Utilities;
using ITTasks.Models.DTOS.Reports.Reporting;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Parameters;
using ITTasks.Statics;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Linq.Dynamic.Core;
using System.Runtime.Loader;

namespace ITTasks.Repositories.Tasks
{
	public class TaskRepository : ITaskRepository
	{
		private readonly ITDbContext _dbContext;

		public TaskRepository(ITDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ITTask> CreateTaskAsync(ITTaskCreateDto task, DateTime createdTime)
		{
			var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == Guid.Parse(task.UserId));
			if (user == null)
				return null;

			var taskType = await _dbContext.TasksType.SingleOrDefaultAsync(x => x.Id == task.TaskTypeId);
			if (taskType == null)
				return null;


			var newTask = new ITTask
			{
				UserId = Guid.Parse(task.UserId),
				StartDate = createdTime,
				EndDate = createdTime.AddMinutes(task.Duration),
				Duration = task.Duration,
				Description = task.Description,
				CreateDate = DateTime.Now,
				ITTaskTypeId = task.TaskTypeId,
				SprintId = task.SprintId,
				UnitId = task.UnitId
			};


			var conflict = await _dbContext.Tasks
				.FirstOrDefaultAsync(x =>
				x.UserId == newTask.UserId &&
				(newTask.StartDate >= x.StartDate && newTask.StartDate <= x.EndDate)
				||
				(newTask.EndDate >= x.StartDate && newTask.EndDate <= x.EndDate)
				);

			if (conflict != null)
			{
				var ex = new Exception();
				ex.Data["conflict"] = "conflict";
				ex.Data["Id"] = conflict.Id;

				throw ex;
			}


			var taskModel = await _dbContext.Tasks.AddAsync(newTask);



			await _dbContext.SaveChangesAsync();

			return taskModel.Entity;
		}

		public async Task<bool> DeleteTaskAsync(Guid id)
		{
			if (id == Guid.Empty)
				return false;
			var task = await _dbContext.Tasks.SingleOrDefaultAsync(x => x.Id == id);
			if (task == null)
				return false;

			_dbContext.Tasks.Remove(task);
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public class SprintsReportViewModel
		{
            public Guid SprintId { get; set; }
			public string SprintTitle { get; set; }
            public int SumOfTimeOfTasks { get; set; }
        }

		public class UsersReportViewModel
		{
			public Guid UserId { get; set; }
			public string UserName { get; set; }
			public int SumOfTimeOfTasks { get; set; }
		}

		public class TaskTypesReportViewModel
		{
			public Guid TaskTypeId { get; set; }
			public string TaskTypeTitle { get; set; }
			public int SumOfTimeOfTasks { get; set; }
		}

		public class UnitsReportViewModel
		{
			public int UnitId { get; set; }
			public string UnitName { get; set; }
			public int SumOfTimeOfTasks { get; set; }
		}


		public class ReportViewModel
		{
			public List<UsersReportViewModel> UsersReport { get; set; }
			public List<SprintsReportViewModel> SprintsReport{ get; set; }
			public List<TaskTypesReportViewModel> TaskTypesReport { get; set; }
			public List<UnitsReportViewModel> UnitsReport { get; set; }
		}


		public async Task<ReportViewModel> GetAllForReportingAsync()
		{
			var sprintReport = await _dbContext.Tasks
				.Include(sp => sp.Sprint)
				.GroupBy(t => t.SprintId)
				.Select(groupedTasks => new SprintsReportViewModel
				{
					SprintId = groupedTasks.Key,
					SprintTitle = groupedTasks.First().Sprint.Title,
					SumOfTimeOfTasks = groupedTasks.Sum(t => t.Duration),
				}).ToListAsync();

			var usersReport = await _dbContext.Tasks
				.Include(u => u.User)
				.GroupBy(t => t.UserId)
				.Select(usersReport => new UsersReportViewModel
				{
					UserId = usersReport.Key,
					UserName = usersReport.First().User.FullName,
					SumOfTimeOfTasks = usersReport.Sum(t => t.Duration),
				}).ToListAsync();

			var taskTypesReport = await _dbContext.Tasks
				.Include(u => u.ITTaskType)
				.GroupBy(t => t.ITTaskTypeId)
				.Select(typesReport => new TaskTypesReportViewModel
				{
					TaskTypeId = typesReport.Key,
					TaskTypeTitle = typesReport.First().ITTaskType.Title,
					SumOfTimeOfTasks = typesReport.Sum(t => t.Duration),
				}).ToListAsync();

			var unitsReport = await _dbContext.Tasks
				.GroupBy(t => t.UnitId)
				.Select(unitReport => new UnitsReportViewModel
				{
					UnitId = unitReport.Key,
					UnitName = unitReport.First().UnitId.GetUnitName(),
					SumOfTimeOfTasks = unitReport.Sum(t => t.Duration),
				}).ToListAsync();


			ReportViewModel result = new ReportViewModel()
			{
				SprintsReport = sprintReport,
				UsersReport = usersReport,
				TaskTypesReport = taskTypesReport,
				UnitsReport = unitsReport,
			};

			return result;
		}

		public async Task<List<ITTask>> GetAllTaskForUserAsync(Guid userId, List<string> sprintIds)
		{
			if (userId == Guid.Empty)
				return null;

			var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
			if (user == null)
				return null;

			var tasks = await _dbContext.Tasks.Where(x =>
			(sprintIds.Any() ? sprintIds.Select(si => Guid.Parse(si)).Contains(x.SprintId) : true))
				.Include(x => x.User)
				.Include(x => x.ITTaskType)
				.Include(x => x.Sprint)
				.Where(x => x.UserId == userId)
				.ToListAsync();

			return tasks;
		}

		public async Task<PagedList<ITTask>> GetAllTasksAsync(TaskParameters param)
		{
			var startDate = DateTimeUtility.UnixToDateTime(param.StartDate);
			var endDate = DateTimeUtility.UnixToDateTime(param.EndDate);

			var taskCount = await _dbContext.Tasks.CountAsync();

			var allTasks = await _dbContext.Tasks.
				Include(u => u.User)
				.Include(t => t.ITTaskType)
				.Include(sp => sp.Sprint)
				.OrderByDescending(tsk => tsk.StartDate)
				.ToListAsync();

			return new PagedList<ITTask>(allTasks, taskCount, param.PageNumber, param.PageSize);
		}

		public async Task<List<ITTask>> GetAllTaskWithOutPaging()
		{
			return await _dbContext.Tasks
				.Include(x => x.User)
				.Include(x => x.ITTaskType)
				.Include(x => x.Sprint)
				.OrderBy(x => x.Sprint)
				.ThenBy(x => x.StartDate)
				.ThenBy(x => x.User.FullName)
				.ToListAsync();
		}

		public async Task<ITTask> GetTaskByIdAsync(Guid id)
		{
			if (id == Guid.Empty)
				return null;

			return await _dbContext.Tasks
				.Include(t => t.ITTaskType)
				.Include(t => t.User)
				.Include(t => t.Sprint)
				.SingleOrDefaultAsync(t => t.Id == id);
		}

		public async Task<List<ITTask>> GetTasksForReporting(ReportingSearchDto searchRequest)
		{
			var tasks = await _dbContext.Tasks
				.Include(sp => sp.Sprint)
				.Include(u => u.User)
				.Include(tp => tp.ITTaskType)
				.Where(t =>
			(searchRequest.SprintIds.Any() ? searchRequest.SprintIds.Select(si => Guid.Parse(si)).Contains(t.SprintId) : true)
			&&
			(searchRequest.UserIds.Any() ? searchRequest.UserIds.Select(ui => Guid.Parse(ui)).Contains(t.UserId) : true)
			).ToListAsync();

			return tasks;
		}

		public async Task<ITTask> UpdateTaskAsync(ITTaskUpdateDto task, DateTime modifiedDate)
		{
			if (task == null)
				return null;

			if (modifiedDate == DateTime.MinValue)
				return null;

			if (task.Id == Guid.Empty && task.SprintId == Guid.Empty && task.TaskTypeId == Guid.Empty)
				return null;

			var unit = UnitsTypes.GetUnitName(task.UnitId);
			if (unit == null)
				return null;

			var taskFromDb = await GetTaskByIdAsync(task.Id);
			if (taskFromDb == null)
				return null;

			taskFromDb.SprintId = task.SprintId;
			taskFromDb.ITTaskTypeId = task.TaskTypeId;
			taskFromDb.Duration = task.Duration;
			taskFromDb.StartDate = modifiedDate;
			taskFromDb.Description = task.Description;
			taskFromDb.UnitId = task.UnitId;

			var taskAfterUpdated = _dbContext.Tasks.Update(taskFromDb);
			if (taskAfterUpdated.Entity == null)
				return null;

			await _dbContext.SaveChangesAsync();

			return taskAfterUpdated.Entity;
		}

	}
}
