using ITTasks.DataLayer;
using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS.Users;
using ITTasks.Models.Errors;
using Microsoft.EntityFrameworkCore;

namespace ITTasks.Repositories.Users
{
	public class UserRepository : IUserRepository
	{
		private readonly ITDbContext _dbContext;

		public UserRepository(ITDbContext dbContext)
		{
			_dbContext = dbContext;
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
			var usersAfterAdd = await _dbContext.Users.AddAsync(new User
			{
				FullName = user.FullName,
				IsActive = false,
				CreatedTime = DateTime.Now,
				UpdatedTime = DateTime.MinValue,
			});

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

			userFromFDb.FullName = user.FullName;
			userFromFDb.UpdatedTime = DateTime.Now;

			var userForReturn = _dbContext.Users.Update(userFromFDb);

			await _dbContext.SaveChangesAsync();

			return userForReturn.Entity;

		}
	}
}
