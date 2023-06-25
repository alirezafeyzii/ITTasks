using ITTasks.DataLayer.Entities;

namespace ITTasks.Repositories.Roles
{
	public interface IRoleRepository
	{
		public Task<Role> CreateRoleAsync(string type);
		public Task<Role> GetRoleByTypeAsync(string type);
		public Task<List<Role>> GetAllRoles();
		public Task CreateGroupRoleAsync(List<string> types);
	}
}
