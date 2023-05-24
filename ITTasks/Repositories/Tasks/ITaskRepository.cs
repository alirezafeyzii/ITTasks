using ITTasks.DataLayer.Entities;
using ITTasks.Infrastructure.Helper;
using ITTasks.Models.DTOS.Reports.Reporting;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Parameters;
using static ITTasks.Repositories.Tasks.TaskRepository;

namespace ITTasks.Repositories.Tasks
{
    public interface ITaskRepository
    {
        public Task<PagedList<ITTask>> GetAllTasksAsync(TaskParameters param);
        public Task<ITTask> CreateTaskAsync(ITTaskCreateDto task, DateTime createdTime);
        public Task<bool> DeleteTaskAsync(Guid id);
        public Task<List<ITTask>> GetAllTaskWithOutPaging();
        public Task<ITTask> UpdateTaskAsync(ITTaskUpdateDto task, DateTime modifiedDate);
        public Task<ITTask> GetTaskByIdAsync(Guid id);
        public Task<List<ITTask>> GetAllTaskForUserAsync(Guid userId, List<string> sprintIds);
        public Task<List<ITTask>> GetTasksForReporting(ReportingSearchDto searchRequest);
        public Task<ReportViewModel> GetAllForReportingAsync();
	}
}
