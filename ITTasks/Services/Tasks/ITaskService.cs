using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS;
using ITTasks.Models.DTOS.Reports.Reporting;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Parameters;
using static ITTasks.Repositories.Tasks.TaskRepository;

namespace ITTasks.Services.Tasks
{
    public interface ITaskService
    {
        public Task<ITTaskDto> CreateTaskAsync(ITTaskCreateDto task);
        public Task<List<ITTaskDto>> GetAllTasksAsync(TaskParameters param);
        public Task<bool> DeleteTaskAsync(Guid id);
        public Task<List<ITTaskDto>> GetAllWithOutPaging();
        public Task<ITTaskDto> UpdateTaskAsync(ITTaskUpdateDto task);
        public Task<ITTaskDto> GetTaskByIdAsync(Guid id);
        public Task<List<ITTaskDto>> GetAllTaskForUserAsync(Guid id, List<string> sprintIds);
		public Task<List<ITTaskDto>> GetTasksForReportingAsync(ReportingSearchDto searchRequest);
        public Task<ReportViewModel> GetReporting();
	}
}
