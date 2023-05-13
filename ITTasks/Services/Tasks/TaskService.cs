using ITTasks.Infrastructure.Extentions;
using ITTasks.Infrastructure.Helper;
using ITTasks.Infrastructure.Utilities;
using ITTasks.Models.DTOS;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Errors;
using ITTasks.Models.Parameters;
using ITTasks.Repositories;
using ITTasks.Repositories.Tasks;

namespace ITTasks.Services.Tasks
{
	public class TaskService : ITaskService
	{
		private readonly ITaskRepository _taskRepository;

		public TaskService(ITaskRepository taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public async Task<ITTaskDto> CreateTaskAsync(ITTaskCreateDto task)
		{
			try
			{
				if (task == null)
				{
					return new ITTaskDto
					{
						ErrorCode = (int)ErrorCodes.NullObjectError,
						ErrorMessage = ErrorMessages.NullInputParameters
					};
				}

				var createdDate = task.Date.UnixToDateTime();

				if (createdDate == new DateTime(1, 1, 1))
				{
					return new ITTaskDto()
					{
						ErrorCode = (int)ErrorCodes.NotAllowedDateTimeFormat,
						ErrorMessage = ErrorMessages.NotAllowedDateTimeFormat
					};
				}

				var taskModel = await _taskRepository.CreateTaskAsync(task, createdDate);
				if (taskModel == null)
				{
					return new ITTaskDto
					{
						ErrorCode = (int)ErrorCodes.DatabaseError,
						ErrorMessage = ErrorMessages.DatabaseError
					};
				}

                return new ITTaskDto
				{
					Id = taskModel.Id,
					UserId = taskModel.UserId,
					Date = taskModel.Date,
					HourAmount = taskModel.HourAmount,
					Description = taskModel.Description,
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError,
					User = new Models.DTOS.Users.UserDto
					{
						Id = taskModel.User.Id,
						FullName = taskModel.User.FullName,
						CreatedTime = taskModel.User.CreatedTime,
						UpdatedTime = taskModel.User.UpdatedTime,
					},
					TaskType = new Models.DTOS.Tasks.TasksType.ITTaskTypeDto
					{
						Id = taskModel.ITTaskType.Id,
						Title = taskModel.ITTaskType.Title,
						CreateTime = taskModel.ITTaskType.CreatedDate,
						UpdateTime = taskModel.ITTaskType.UpdatedDate
					}
				};
			}
			catch (Exception)
			{
				return new ITTaskDto
				{
					ErrorCode = (int)ErrorCodes.ServerError,
					ErrorMessage = ErrorMessages.ServerError
				};
			}

		}

		public async Task<bool> DeleteTaskAsync(Guid id)
		{
			if (id == Guid.Empty)
				return false;

			var taskAfterDeleted = await _taskRepository.DeleteTaskAsync(id);
			if(taskAfterDeleted == false)
				return false;

			return true;
		}

		public async Task<List<ITTaskDto>> GetAllTasksAsync(TaskParameters param)
		{
			var taskGroup = new List<ITTaskDto>();

			var tasks = await _taskRepository.GetAllTasksAsync(param);
			
			var pageInfo = new PageInfo
			{
				CurrentPage = tasks.CurrentPage,
				ItemsPerPage = tasks.PageSize,
				TotalItems = tasks.TotalCount
			};

			foreach (var task in tasks)
			{
				taskGroup.Add(new ITTaskDto
				{
					Id = task.Id,
					UserId = task.UserId,
					Date = task.Date,
					HourAmount = task.HourAmount,
					Description = task.Description,
					PersianDate = DateTimeExtention.ToPersianWithOutTime(task.Date),
					User = new Models.DTOS.Users.UserDto
					{
						Id = task.User.Id,
						FullName = task.User.FullName,
						CreatedTime = task.User.CreatedTime,
						UpdatedTime = task.User.UpdatedTime,
					},
					PageInfo = pageInfo,
					TaskType = new Models.DTOS.Tasks.TasksType.ITTaskTypeDto
					{
						Id = task.ITTaskType.Id,
						Title = task.ITTaskType.Title,
						CreateTime = task.ITTaskType.CreatedDate,
						UpdateTime = task.ITTaskType.UpdatedDate
					},
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError
				});
			}

			return taskGroup;
		}

		public async Task<List<ITTaskDto>> GetAllWithOutPaging()
		{
			var taskGroup = new List<ITTaskDto>();
			var tasks = await _taskRepository.GetAllTaskWithOutPaging();
			foreach (var task in tasks)
			{
				taskGroup.Add(new ITTaskDto
				{
					Id = task.Id,
					UserId = task.UserId,
					Date = task.Date,
					HourAmount = task.HourAmount,
					Description = task.Description,
					PersianDate = DateTimeExtention.ToPersianWithOutTime(task.Date),
					User = new Models.DTOS.Users.UserDto
					{
						Id = task.User.Id,
						FullName = task.User.FullName,
						CreatedTime = task.User.CreatedTime,
						UpdatedTime = task.User.UpdatedTime,
					},
					TaskType = new Models.DTOS.Tasks.TasksType.ITTaskTypeDto
					{
						Id = task.ITTaskType.Id,
						Title = task.ITTaskType.Title,
						CreateTime = task.ITTaskType.CreatedDate,
						UpdateTime = task.ITTaskType.UpdatedDate
					},
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError
				});
			}

			return taskGroup;
		}
	}
}
