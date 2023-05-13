using ITTasks.DataLayer.Entities;
using ITTasks.Infrastructure.Helper;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Parameters;

namespace ITTasks.Repositories.Tasks
{
    public interface ITaskRepository
    {
        public Task<PagedList<ITTask>> GetAllTasksAsync(TaskParameters param);
        public Task<ITTask> CreateTaskAsync(ITTaskCreateDto task, DateTime createdTime);
        public Task<bool> DeleteTaskAsync(Guid id);
        public Task<List<ITTask>> GetAllTaskWithOutPaging();
    }
}
