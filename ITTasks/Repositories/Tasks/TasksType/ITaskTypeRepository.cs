using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.DTOS.Tasks.TasksType;

namespace ITTasks.Repositories.Tasks.TasksType
{
	public interface ITaskTypeRepository
	{
		public Task<ITTaskType> CreateAsync(ITTaskTypeCreateDto taskType);
		public Task<List<ITTaskType>> GetAllAsync();
		public Task<bool> DeleteAsync(Guid id);
		public Task<ITTaskType> GetByIdAsync(Guid id);
	}
}
