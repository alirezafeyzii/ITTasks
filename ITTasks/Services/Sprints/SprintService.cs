using ITTasks.Infrastructure.Extentions;
using ITTasks.Infrastructure.Utilities;
using ITTasks.Models.DTOS.Sprints;
using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.Errors;
using ITTasks.Repositories.Sprints;
using ITTasks.Repositories.Tasks.TasksType;
using System.Threading.Tasks;

namespace ITTasks.Services.Sprints
{
	public class SprintService : ISprintService
	{
		private readonly ISprintRepository _sprintRepository;

		public SprintService(ISprintRepository sprintRepository)
		{
			_sprintRepository = sprintRepository;

		}

		public async Task<SprintDto> CreateAsync(CreateSprintDto sprint)
		{
			if(sprint.Title == null)
			{
				return new SprintDto
				{
					ErrorCode = (int)ErrorCodes.NullObjectError,
					ErrorMessage = ErrorMessages.NullInputParameters
				};
			}

			var startDate = sprint.StartDate.UnixToDateTime();

			var endDate = sprint.EndDate.UnixToDateTime();

			if (startDate == new DateTime(1, 1, 1))
			{
				return new SprintDto
				{
					ErrorCode = (int)ErrorCodes.NotAllowedDateTimeFormat,
					ErrorMessage = ErrorMessages.NotAllowedDateTimeFormat
				};
			}

			var sprintExists = await _sprintRepository.GetByTitleAsync(sprint.Title);
			if(sprintExists != null)
			{
				return new SprintDto
				{
					ErrorCode = (int)ErrorCodes.ConflictSprint,
					ErrorMessage = ErrorMessages.ConflictSprint
				};
			}

			var sprintFromRepo = await _sprintRepository.CreateAsync(sprint, startDate, endDate);

			if(sprintFromRepo == null)
			{
				return new SprintDto
				{
					ErrorCode = (int)ErrorCodes.DatabaseError,
					ErrorMessage = ErrorMessages.DatabaseError
				};
			}

			return new SprintDto
			{
				Title = sprintFromRepo.Title,
				StartDateTime = sprintFromRepo.StartDate,
				EndDateTime = sprintFromRepo.EndDate,
				StartDate = sprint.StartDate,
				EndDate = sprint.EndDate,
				ErrorCode = (int)ErrorCodes.NoError,
				ErrorMessage = ErrorMessages.NoError
			};
		}

		public async Task DleteSprintAsync(Guid id)
		{
			await _sprintRepository.DeleteSprintAsync(id);
		}

		public async Task<List<SprintDto>> GetAllAsync()
		{
			var sprintGroup = new List<SprintDto>();

			var allSprints = await _sprintRepository.GetAllAsync();

			foreach (var sprint in allSprints)
			{
				sprintGroup.Add(new SprintDto
				{
					Id = sprint.Id,
					Title = sprint.Title,
					StringStartDate = DateTimeExtention.ToPersianWithOutTime(sprint.StartDate),
					StringEndDate = DateTimeExtention.ToPersianWithOutTime(sprint.EndDate)
				});
			}

			return sprintGroup;
		}

		public async Task<SprintDto> GetByTitleAsync(string title)
		{
			try
			{
				if(title == null)
				{
					return new SprintDto
					{
						ErrorCode = (int)ErrorCodes.NullObjectError,
						ErrorMessage = ErrorMessages.NullInputParameters
					};
				}

				var sprint = await _sprintRepository.GetByTitleAsync(title);
				if(sprint == null)
				{
					return new SprintDto
					{
						ErrorCode = (int)ErrorCodes.SprintNotFound,
						ErrorMessage = ErrorMessages.SprintNotFound
					};
				}

				return new SprintDto
				{
					Id = sprint.Id,
					Title = sprint.Title,
					StringStartDate = DateTimeExtention.ToPersianWithOutTime(sprint.StartDate),
					StringEndDate = DateTimeExtention.ToPersianWithOutTime(sprint.EndDate),
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError
				};
			}
			catch (Exception)
			{
				return new SprintDto
				{
					ErrorCode = (int)ErrorCodes.DatabaseError,
					ErrorMessage = ErrorMessages.DatabaseError
				};
			}
		}
	}
}
