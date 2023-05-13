using ITTasks.Models.DTOS.Tasks;
using ITTasks.Models.DTOS.Users;
using ITTasks.Models.Errors;
using ITTasks.Repositories.Users;

namespace ITTasks.Services.Users
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<UserDto> CreateUserAsync(CreateUserDto user)
		{
			try
			{
				if (user.FullName == null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.UserFullNameError,
						ErrorMessage = ErrorMessages.UserFullNameError,
					};
				}

				var userFromRepo = await _userRepository.CreateUserAsync(user);

				if (userFromRepo == null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.DatabaseError,
						ErrorMessage = ErrorMessages.DatabaseError
					};
				}

				return new UserDto
				{
					Id = userFromRepo.Id,
					FullName = userFromRepo.FullName,
					IsActive = userFromRepo.IsActive,
					CreatedTime = userFromRepo.CreatedTime,
					UpdatedTime = userFromRepo.UpdatedTime,
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError
				};
			}
			catch (Exception)
			{
				return new UserDto
				{
					ErrorCode = (int)ErrorCodes.ServerError,
					ErrorMessage = ErrorMessages.ServerError
				};
			}
		}

		public async Task<List<UserDto>> GetAllActiveUsersAsync()
		{
			var usersGroup = new List<UserDto>();

			var users = await _userRepository.GetAllActiveUsersAsync();

			foreach (var user in users)
			{
				usersGroup.Add(new UserDto
				{
					Id = user.Id,
					FullName = user.FullName,
					IsActive = user.IsActive,
					CreatedTime = user.CreatedTime,
					UpdatedTime = user.UpdatedTime,
				});
			}

			return usersGroup;
		}

		public async Task<List<UserDto>> GetAllUsersAsync()
		{
			var usersGroup = new List<UserDto>();

			var users = await _userRepository.GetAllUsersAsync();

			foreach (var user in users)
			{
				usersGroup.Add(new UserDto
				{
					Id = user.Id,
					FullName = user.FullName,
					IsActive = user.IsActive,
					CreatedTime = user.CreatedTime,
					UpdatedTime = user.UpdatedTime,
				});
			}

			return usersGroup;
		}

		public async Task<UserDto> GetUserByIdAsync(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.UserIdError,
						ErrorMessage = ErrorMessages.UserIdError
					};
				}

				var user = await _userRepository.GetUserByIdAsync(id);
				if (user == null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.UserNotFound,
						ErrorMessage = ErrorMessages.UserNotFound
					};
				}

				return new UserDto
				{
					Id = user.Id,
					FullName = user.FullName,
					IsActive = user.IsActive,
					CreatedTime = user.CreatedTime,
					UpdatedTime = user.UpdatedTime,
				};
			}
			catch (Exception)
			{
				return new UserDto
				{
					ErrorCode = (int)ErrorCodes.ServerError,
					ErrorMessage = ErrorMessages.ServerError
				};
			}
		}
	}
}
