using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.DTOS.Tasks.TasksType;

namespace ITTasks.Services.Tasks.TasksType
{
	public interface ITaskTypeService
	{
		public Task<ITTaskTypeDto> CreateAsync(ITTaskTypeCreateDto taskType);
		public Task<List<ITTaskTypeDto>> GetAllAsync();
	}
}
