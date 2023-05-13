﻿using ITTasks.DataLayer;
using ITTasks.DataLayer.Entities;
using ITTasks.Infrastructure.Helper;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Parameters;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
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
			if (task == null)
				return null;

			var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == Guid.Parse(task.UserId));
			if (user == null)
				return null;

			var taskType = await _dbContext.TasksType.SingleOrDefaultAsync(x => x.Id == task.TaskTypeId);
			if (taskType == null)
				return null;

				var taskModel = await _dbContext.Tasks.AddAsync(new ITTask
			{
				UserId = Guid.Parse(task.UserId),
				Date = createdTime,
				HourAmount = task.HourAmount,
				Description = task.Description,
				CreateDate = DateTime.Now,
				ITTaskTypeId = task.TaskTypeId,
				SprintId = task.SprintId,
			});
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

		public async Task<PagedList<ITTask>> GetAllTasksAsync(TaskParameters param)
		{
			var taskCount = await _dbContext.Tasks.CountAsync();

			var allTasks = await _dbContext.Tasks.
				Include(t => t.User)
				.Include(x => x.ITTaskType)
				.OrderByDescending(x => x.Date)
				.Skip(param.PageSize
				* (param.PageNumber - 1))
				.Take(param.PageSize)
				.ToListAsync();

			if (param.SearchParameter != null)
			{
				allTasks = allTasks
					.Where(t => t.Description.Contains(param.SearchParameter))
					.ToList();
			}

			return new PagedList<ITTask>(allTasks, taskCount,param.PageNumber,param.PageSize);
		}

		public async Task<List<ITTask>> GetAllTaskWithOutPaging()
		{
			return await _dbContext.Tasks
				.Include(x => x.User)
				.Include(x => x.ITTaskType)
				.ToListAsync();
		}
	}
}
