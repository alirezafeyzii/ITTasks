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
			var taskTypeAfterAdded = await _dbContext.TasksType.AddAsync(new ITTaskType
			{
				Title = taskType.Title,
				CreatedDate = DateTime.UtcNow,
				UpdatedDate = DateTime.MinValue
			});

			await _dbContext.SaveChangesAsync();

			return taskTypeAfterAdded.Entity;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var taskType = await GetByIdAsync(id);
			if (taskType == null)
				return false;

			_dbContext.TasksType.Remove(taskType);

			await _dbContext.SaveChangesAsync();

			return true;
		}

		public async Task<List<ITTaskType>> GetAllAsync()
		{
			return await _dbContext.TasksType.ToListAsync();
		}

		public async Task<ITTaskType> GetByIdAsync(Guid id)
		{
			var taskType = await _dbContext.TasksType.SingleOrDefaultAsync(tt => tt.Id == id);
			if (taskType == null)
				return null;

			return taskType;
		}

		public async Task<ITTaskType> GetByTitleAsync(string title)
		{
			var taskType = await _dbContext.TasksType.SingleOrDefaultAsync(x => x.Title.ToLower() == title.ToLower());
			if (taskType == null)
				return null;

			return taskType;
		}
	}
}
