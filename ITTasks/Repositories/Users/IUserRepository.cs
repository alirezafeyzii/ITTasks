using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS.Users;

namespace ITTasks.Repositories.Users
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(CreateUserDto user);
        public Task<List<User>> GetAllUsersAsync();
		public Task<List<User>> GetAllActiveUsersAsync();
        public Task<User> GetUserByIdAsync(Guid id);
        public Task<User> UpdateUserAsync(UserDto user);
        public Task<User> ChangeUserStatusAsync(Guid id, bool status);
        public Task<User> GetUserByNameAsync(string name);
	}
}
