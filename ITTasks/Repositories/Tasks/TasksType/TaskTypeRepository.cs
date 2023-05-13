using ITTasks.DataLayer;
using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS.Tasks.TasksType;
using Microsoft.EntityFrameworkCore;

namespace ITTasks.Repositories.Tasks.TasksType
{
	public class TaskTypeRepository : ITaskTypeRepository
	{
		private readonly ITDbContext _dbContext;

		public TaskTypeRepository(ITDbContext dbContext)
		{
			_dbContext = dbContext;
		}


		public async Task<ITTaskType> CreateAsync(ITTaskTypeCreateDto taskType)
		{
			if (taskType.Title == null)
				return null;

			var taskTypeAfterAdded = await _dbContext.TasksType.AddAsync(new ITTaskType
			{
				Title = taskType.Title,
				CreatedDate = DateTime.UtcNow,
				UpdatedDate = DateTime.MinValue
			});

			await _dbContext.SaveChangesAsync();

			return taskTypeAfterAdded.Entity;
		}

		public async Task<List<ITTaskType>> GetAllAsync()
		{
			return await _dbContext.TasksType.ToListAsync();
		}
	}
}
