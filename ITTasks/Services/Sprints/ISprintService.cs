using ITTasks.Models.DTOS.Sprints;
using ITTasks.Models.DTOS.Tasks.TasksType;

namespace ITTasks.Services.Sprints
{
	public interface ISprintService
	{
		public Task<SprintDto> CreateAsync(CreateSprintDto sprint);
		public Task<List<SprintDto>> GetAllAsync();
	}
}
