using ITTasks.DataLayer.Entities;
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

		public async Task<UserDto> ChangeUserStatusAsync(Guid id, bool status)
		{
			try
			{
				if (id == Guid.Empty)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.UserIdError,
						ErrorMessage = ErrorMessages.UserIdError,
					};
				}

				UserDto userFromRepo = await _userRepository.ChangeUserStatusAsync(id, status);
				if (userFromRepo == null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.UserNotFound,
						ErrorMessage = ErrorMessages.UserNotFound,
					};
				}

				return userFromRepo;
			}
			catch (Exception)
			{
				return new UserDto
				{
					ErrorCode = (int)ErrorCodes.ServerError,
					ErrorMessage = ErrorMessages.ServerError,
				};
			}
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

				var userExists = await _userRepository.GetUserByNameAsync(user.FullName);
				if (userExists != null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.ConflictUser,
						ErrorMessage = ErrorMessages.ConflictUser
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

		public async Task<UserDto> GetUserByNameAsync(string name)
		{
			try
			{
				if (name == null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.NullObjectError,
						ErrorMessage = ErrorMessages.NullInputParameters
					};
				}

				var userFromRepo = await _userRepository.GetUserByNameAsync(name);
				if (userFromRepo == null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.UserNotFound,
						ErrorMessage = ErrorMessages.UserNotFound
					};
				}

				return new UserDto
				{
					Id = userFromRepo.Id,
					FullName = userFromRepo.FullName,
					IsActive = userFromRepo.IsActive,
					CreatedTime = userFromRepo.CreatedTime,
					UpdatedTime= userFromRepo.UpdatedTime,
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError
				};
			}

			catch (Exception)
			{
				return new UserDto
				{
					ErrorCode = (int)ErrorCodes.DatabaseError,
					ErrorMessage = ErrorMessages.DatabaseError
				};
			}
		}

		public async Task<UserDto> UpdatUserAsync(UserDto user)
		{
			try
			{
				if(user == null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.NullObjectError,
						ErrorMessage = ErrorMessages.NullInputParameters
					};
				}

				if (user.FullName == null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.UserFullNameError,
						ErrorMessage = ErrorMessages.UserFullNameError
					};
				}

				if (user.Id == Guid.Empty)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.UserIdError,
						ErrorMessage = ErrorMessages.UserIdError
					};
				}

				var userExists = await _userRepository.GetUserByNameAsync(user.FullName);
				if (userExists != null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.UserIdError,
						ErrorMessage = ErrorMessages.UserIdError
					};
				}

				var userFromRepo = await _userRepository.UpdateUserAsync(user);
				if(userFromRepo == null)
				{
					return new UserDto
					{
						ErrorCode = (int)ErrorCodes.DatabaseError,
						ErrorMessage = ErrorMessages.DatabaseError
					};
				}

				return new UserDto
				{
					FullName = userFromRepo.FullName,
					Id = userFromRepo.Id,
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
					ErrorCode = (int)ErrorCodes.DatabaseError,
					ErrorMessage = ErrorMessages.DatabaseError
				};
			}
		}
	}
}
