using ITTasks.DataLayer.Entities;
using ITTasks.Repositories.Roles;

namespace ITTasks.Services.Roles
{
	public class RoleService : IRoleService
	{
		private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
			_roleRepository = roleRepository;
		}

        public async Task<Role> CreateRole(string type)
		{
			var role = await _roleRepository.CreateRoleAsync(type);
			return role;
		}

		public Task<Role> GetRoleByType(string type)
		{
			throw new NotImplementedException();
		}
	}
}
