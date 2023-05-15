using ITTasks.Models.DTOS.Tasks.TasksType;
using ITTasks.Models.Errors;
using ITTasks.Repositories.Tasks.TasksType;

namespace ITTasks.Services.Tasks.TasksType
{
	public class TaskTypeService : ITaskTypeService
	{
		private readonly ITaskTypeRepository _taskTypeRepository;

        public TaskTypeService(ITaskTypeRepository taskTypeRepository)
        {
			_taskTypeRepository = taskTypeRepository;

		}

        public async Task<ITTaskTypeDto> CreateAsync(ITTaskTypeCreateDto taskType)
		{
			try
			{
				if (taskType.Title == null)
				{
					return new ITTaskTypeDto
					{
						ErrorCode = (int)ErrorCodes.NullObjectError,
						ErrorMessage = ErrorMessages.NullInputParameters
					};
				}

				var taskTypeFromRepo = await _taskTypeRepository.CreateAsync(taskType);

				if (taskTypeFromRepo == null)
				{
					return new ITTaskTypeDto
					{
						ErrorCode = (int)ErrorCodes.ServerError,
						ErrorMessage = ErrorMessages.ServerError
					};
				}

				return new ITTaskTypeDto
				{
					Id = taskTypeFromRepo.Id,
					Title = taskTypeFromRepo.Title,
					CreateTime = taskTypeFromRepo.CreatedDate,
					UpdateTime = taskTypeFromRepo.UpdatedDate,
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError
				};
			}
			catch (Exception)
			{
				return new ITTaskTypeDto
				{
					ErrorCode = (int)ErrorCodes.ServerError,
					ErrorMessage = ErrorMessages.ServerError
				};
			}
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			if (id == Guid.Empty)
				return false;

			var result = await _taskTypeRepository.DeleteAsync(id);
			if(result == false)
				return false;

			return true;
		}

		public async Task<List<ITTaskTypeDto>> GetAllAsync()
		{
			var taskTypeGroup = new List<ITTaskTypeDto>();

			var taskTypes = await _taskTypeRepository.GetAllAsync();

            foreach (var taskType in taskTypes)
            {
				taskTypeGroup.Add(new ITTaskTypeDto
				{
					Id = taskType.Id,
					Title = taskType.Title,
					CreateTime = taskType.CreatedDate,
					UpdateTime = taskType.UpdatedDate
				});
			}

			return taskTypeGroup;
        }
	}
}
