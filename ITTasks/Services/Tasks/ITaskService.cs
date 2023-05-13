using ITTasks.Models.DTOS;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Parameters;

namespace ITTasks.Services.Tasks
{
    public interface ITaskService
    {
        public Task<ITTaskDto> CreateTaskAsync(ITTaskCreateDto task);
        public Task<List<ITTaskDto>> GetAllTasksAsync(TaskParameters param);
        public Task<bool> DeleteTaskAsync(Guid id);
        public Task<List<ITTaskDto>> GetAllWithOutPaging();
    }
}
