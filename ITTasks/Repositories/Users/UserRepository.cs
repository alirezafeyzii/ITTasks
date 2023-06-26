using ITTasks.DataLayer;
using ITTasks.DataLayer.Entities;
using ITTasks.Infrastructure.Utilities;
using ITTasks.Models.DTOS.Users;
using ITTasks.Models.Errors;
using ITTasks.Repositories.Roles;
using ITTasks.Statics.Entities.Roles;
using Microsoft.EntityFrameworkCore;

namespace ITTasks.Repositories.Users
{
	public class UserRepository : IUserRepository
	{
		private readonly ITDbContext _dbContext;
		private readonly IRoleRepository _roleRepository;

		public UserRepository(ITDbContext dbContext, 
			IRoleRepository roleRepository)
		{
			_dbContext = dbContext;
			_roleRepository = roleRepository;
		}

		public async Task<User> ChangeUserStatusAsync(Guid id, bool status)
		{
			var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == id);
			if (user == null)
				return null;

			user.IsActive = status;

			UserDto userDto = user;
			var afterChangedStatus = await UpdateUserAsync(userDto);
			if (afterChangedStatus == null)
				return null;

			return afterChangedStatus;
		}

		public async Task<User> CreateUserAsync(CreateUserDto user)
		{
			var role = await _roleRepository.GetRoleByTypeAsync(RoleTypes.User);

			var userNameNormalize = user.UserName.ToNormalize();
			var emailNormalize = user.Email.ToNormalize();

			var conflict = await _dbContext.Users
				.Where(u => u.NormalizedEmail == emailNormalize ||
				u.NormalizedUserName == userNameNormalize || 
				u.PhoneNumber == user.PhoneNumber)
				.FirstOrDefaultAsync();

			if (conflict is not null)
				return null;

			var userForCreate = new User
			{
				FullName = user.FullName,
				UserName = user.UserName,
				NormalizedUserName = userNameNormalize,
				Email = user.Email,
				NormalizedEmail = emailNormalize,
				PhoneNumber = user.PhoneNumber,
				EmailConfirmed = false,
				PhoneNumberConfirmed = false,
				IsActive = false,
				PasswordHash = user.Password.Encrypt(),
				Token = user.Token != null ? user.Token : string.Empty,
				RoleId = role.Id,
				CreatedTime = DateTime.Now,
				UpdatedTime = DateTime.MinValue,
			};

			var usersAfterAdd = await _dbContext.Users.AddAsync(userForCreate);

			await _dbContext.SaveChangesAsync();

			return usersAfterAdd.Entity;

		}

		public async Task<List<User>> GetAllActiveUsersAsync()
		{
			return await _dbContext.Users.Where(u => u.IsActive == true).ToListAsync();
		}

		public async Task<List<User>> GetAllUsersAsync()
		{
			return await _dbContext.Users.ToListAsync();
		}

		public async Task<User> GetUserByIdAsync(Guid id)
		{
			var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == id);
			if (user == null)
				return null;

			return user;
		}

		public async Task<User> GetUserForSignInAsync(string userName, string password)
		{
			var user = await _dbContext.Users
				.Include(x => x.Roles)
				.Where(u => 
				(u.NormalizedUserName == userName.ToNormalize() ||
				u.NormalizedEmail == userName.ToNormalize() ||
				u.PhoneNumber == userName)
				&& 
				u.PasswordHash == password.Encrypt())
				.SingleOrDefaultAsync();

			if (user == null)
				return null;

			return user;
		}

		public async Task<User> GetUserByNameAsync(string name)
		{
			var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.FullName.ToLower() == name.ToLower());
			if (user == null)
				return null;

			return user;
		}

		public async Task<User> UpdateUserAsync(UserDto user)
		{
			var userFromFDb = await GetUserByIdAsync(user.Id);
			if (userFromFDb == null)
				return null;

			userFromFDb.UserName = user.FullName;
			userFromFDb.UpdatedTime = DateTime.Now;

			var userForReturn = _dbContext.Users.Update(userFromFDb);

			await _dbContext.SaveChangesAsync();

			return userForReturn.Entity;

		}
	}
}
