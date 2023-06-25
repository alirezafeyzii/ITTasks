using ITTasks.DataLayer.Entities;

namespace ITTasks.Services.Roles
{
	public interface IRoleService
	{
		public Task<Role> GetRoleByType(string type);
		public Task<Role> CreateRole(string type);
	}
}
