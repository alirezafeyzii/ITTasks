using ITTasks.Infrastructure.Extentions;
using ITTasks.Infrastructure.Helper;
using ITTasks.Infrastructure.Utilities;
using ITTasks.Models.DTOS;
using ITTasks.Models.DTOS.Reports.Reporting;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Errors;
using ITTasks.Models.Parameters;
using ITTasks.Repositories;
using ITTasks.Repositories.Tasks;
using ITTasks.Statics;

using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

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
					Date = taskModel.StartDate,
					Duration = taskModel.Duration,
					Description = taskModel.Description,
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError,
					UnitId = taskModel.UnitId,
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
			catch (Exception ex)
			{

				if (ex.Data["conflict"] == "conflict")
				{
					return new ITTaskDto
					{
						ErrorCode = (int)ErrorCodes.ConflictTask,
						ErrorMessage = ex.Data["Id"].ToString(),
					};
				}

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
			if (taskAfterDeleted == false)
				return false;

			return true;
		}

		public async Task<List<ITTaskDto>> GetAllTaskForUserAsync(Guid id, List<string> sprintIds)
		{
			var allTasks = new List<ITTaskDto>();

			if (id == Guid.Empty)
				return null;

			var tasks = await _taskRepository.GetAllTaskForUserAsync(id, sprintIds);

            foreach (var task in tasks)
            {
				allTasks.Add(new ITTaskDto
				{
					Id = task.Id,
					
					UserId = task.UserId,
					User = task.User,
					Date = task.StartDate,
					Duration = task.Duration,
					Description = task.Description,
					StandardDuration = DateTimeExtention.ToStandardTime(task.Duration),
					PersianDate = DateTimeExtention.ToPersianWithOutTime(task.StartDate),
					UnitId = task.UnitId,
					UnitName = UnitsTypes.GetUnitName(task.UnitId),
					TaskType = new Models.DTOS.Tasks.TasksType.ITTaskTypeDto
					{
						Id = task.ITTaskType.Id,
						Title = task.ITTaskType.Title,
						CreateTime = task.ITTaskType.CreatedDate,
						UpdateTime = task.ITTaskType.UpdatedDate
					},
					Sprint = new Models.DTOS.Sprints.SprintDto
					{
						Id = task.Sprint.Id,
						Title = task.Sprint.Title,
						StartDateTime = task.Sprint.StartDate,
						EndDateTime = task.Sprint.EndDate
					}
				});
            }

			return allTasks;
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
					Date = task.StartDate,
					Duration = task.Duration,
					Description = task.Description,
					StandardDuration = DateTimeExtention.ToStandardTime(task.Duration),
					PersianDate = DateTimeExtention.ToPersianWithOutTime(task.StartDate),
					UnitId = task.UnitId,
					UnitName = UnitsTypes.GetUnitName(task.UnitId),
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
					Sprint = new Models.DTOS.Sprints.SprintDto
					{
						Id = task.Sprint.Id,
						Title = task.Sprint.Title,
						StartDateTime = task.Sprint.StartDate,
						EndDateTime = task.Sprint.EndDate
					},
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError
				});
			}

			return taskGroup;
		}

		public async Task<List<ITTaskDto>> GetAllWithOutPagingAsync()
		{
			var taskGroup = new List<ITTaskDto>();
			var tasks = await _taskRepository.GetAllTaskWithOutPaging();
			foreach (var task in tasks)
			{
				taskGroup.Add(new ITTaskDto
				{
					Id = task.Id,
					UserId = task.UserId,
					Date = task.StartDate,
					Duration = task.Duration,
					StandardDuration = DateTimeExtention.ToStandardTime(task.Duration),
					Description = task.Description,
					PersianDate = DateTimeExtention.ToPersianWithOutTime(task.StartDate),
					UnitId = task.UnitId,
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
					Sprint = new Models.DTOS.Sprints.SprintDto
					{
						Id = task.Sprint.Id,
						Title = task.Sprint.Title,
						StartDateTime = task.StartDate,
						EndDateTime = task.StartDate
					},
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError
				});
			}

			return taskGroup;
		}

		public async Task<TaskRepository.ReportViewModel> GetReportingAsync()
		{
			var rp = await _taskRepository.GetAllForReportingAsync();
			return rp;
		}

		public async Task<ITTaskDto> GetTaskByIdAsync(Guid id)
		{
			if (id == Guid.Empty)
				return new ITTaskDto
				{
					ErrorCode = (int)ErrorCodes.NullObjectError,
					ErrorMessage = ErrorMessages.NullInputParameters
				};

			var task = await _taskRepository.GetTaskByIdAsync(id);
			if (task == null)
			{
				return new ITTaskDto
				{
					ErrorCode = (int)ErrorCodes.ServerError,
					ErrorMessage = ErrorMessages.ServerError
				};
			}

			return new ITTaskDto
			{
				Id = task.Id,
				UserId = task.UserId,
				Date = task.StartDate,
				Duration = task.Duration,
				StandardDuration = DateTimeExtention.ToStandardTime(task.Duration),
				Description = task.Description,
				PersianDate = DateTimeExtention.ToPersianWithOutTime(task.StartDate),
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
				Sprint = new Models.DTOS.Sprints.SprintDto
				{
					Id = task.Sprint.Id,
					Title = task.Sprint.Title,
					StartDateTime = task.StartDate,
					EndDateTime = task.StartDate
				},
				ErrorCode = (int)ErrorCodes.NoError,
				ErrorMessage = ErrorMessages.NoError

			};
		}

		public async Task<List<ITTaskDto>> GetTasksForReportingAsync(ReportingSearchDto searchRequest)
		{
			var taskGroup = new List<ITTaskDto>();
			var tasks = await _taskRepository.GetTasksForReporting(searchRequest);
			foreach (var task in tasks)
			{
				taskGroup.Add(new ITTaskDto
				{
					Id = task.Id,
					UserId = task.UserId,
					Date = task.StartDate,
					Duration = task.Duration,
					StandardDuration = DateTimeExtention.ToStandardTime(task.Duration),
					Description = task.Description,
					PersianDate = DateTimeExtention.ToPersianWithOutTime(task.StartDate),
					UnitId = task.UnitId,
					UnitName = task.UnitId.GetUnitName(),
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
					Sprint = new Models.DTOS.Sprints.SprintDto
					{
						Id = task.Sprint.Id,
						Title = task.Sprint.Title,
						StartDateTime = task.StartDate,
						EndDateTime = task.StartDate
					},
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError
				});
			}
			return taskGroup;
		}

		public async Task<ITTaskDto> UpdateTaskAsync(ITTaskUpdateDto task)
		{
			try
			{
				if (task.Id == Guid.Empty
				&& task.SprintId == Guid.Empty
				&& task.TaskTypeId == Guid.Empty
				&& task.Description == null
				&& task.Duration == 0)
				{
					return new ITTaskDto
					{
						ErrorCode = (int)ErrorCodes.NullObjectError,
						ErrorMessage = ErrorMessages.NullInputParameters
					};
				}

				var modifiedDate = task.Date.UnixToDateTime();

				if (modifiedDate == new DateTime(1, 1, 1))
				{
					return new ITTaskDto()
					{
						ErrorCode = (int)ErrorCodes.NotAllowedDateTimeFormat,
						ErrorMessage = ErrorMessages.NotAllowedDateTimeFormat
					};
				}

				var taskFromRepo = await _taskRepository.UpdateTaskAsync(task, modifiedDate);
				if (taskFromRepo == null)
				{
					return new ITTaskDto
					{
						ErrorCode = (int)ErrorCodes.ServerError,
						ErrorMessage = ErrorMessages.ServerError
					};
				}

				return new ITTaskDto
				{
					Id = taskFromRepo.Id,
					Sprint = new Models.DTOS.Sprints.SprintDto
					{
						Id = taskFromRepo.Sprint.Id,
						Title = taskFromRepo.Sprint.Title,
						StartDateTime = taskFromRepo.Sprint.StartDate,
						EndDateTime = taskFromRepo.Sprint.EndDate,
					},
					TaskType = new Models.DTOS.Tasks.TasksType.ITTaskTypeDto
					{
						Id = taskFromRepo.ITTaskType.Id,
						Title = taskFromRepo.ITTaskType.Title
					},
					User = new Models.DTOS.Users.UserDto
					{
						Id = taskFromRepo.User.Id,
						FullName = taskFromRepo.User.FullName,
					},
					Description = taskFromRepo.Description,
					Duration = taskFromRepo.Duration,
					StandardDuration = DateTimeExtention.ToStandardTime(taskFromRepo.Duration),
					PersianDate = DateTimeExtention.ToPersian(taskFromRepo.StartDate)
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

	}
}
