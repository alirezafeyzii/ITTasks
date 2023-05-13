using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS.Sprints;
using ITTasks.Models.DTOS.Tasks.TasksType;

namespace ITTasks.Repositories.Sprints
{
	public interface ISprintRepository
	{
		public Task<Sprint> CreateAsync(CreateSprintDto sprint, DateTime startDate, DateTime endDate);
		public Task<List<Sprint>> GetAllAsync();
	}
}
