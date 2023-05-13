using ITTasks.DataLayer;
using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS.Users;
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

        public async Task<User> CreateUserAsync(CreateUserDto user)
        {
            if (user.FullName == null)
                return null;

            var userSAfterAdd = await _dbContext.Users.AddAsync(new User
            {
                FullName = user.FullName,
                IsActive = false,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.MinValue,
            });

            await _dbContext.SaveChangesAsync();

            return userSAfterAdd.Entity;

        }

		public async Task<List<User>> GetAllActiveUsersAsync()
		{
            return await _dbContext.Users.Where(u => u.IsActive).ToListAsync();
		}

		public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

		public async Task<User> GetUserByIdAsync(Guid id)
		{
            if (id == Guid.Empty)
                return null;

            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return null;

            return user;
		}
	}
}
