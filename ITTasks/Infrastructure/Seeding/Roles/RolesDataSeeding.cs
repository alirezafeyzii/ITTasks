using ITTasks.DataLayer.Entities;
using ITTasks.Repositories.Roles;
using ITTasks.Statics.Entities.Roles;

namespace ITTasks.Infrastructure.Seeding.Roles
{
	public class RolesDataSeeding
	{
		private readonly IRoleRepository _roleRepository;

		public RolesDataSeeding(IRoleRepository roleRepository)
		{
			_roleRepository = roleRepository;

		}

		public async Task SeedRoleAsync()
		{
			var roleExists = await _roleRepository.GetAllRoles();

			if (roleExists.Count() < 1)
			{
				var roles = new List<string>
				{
					RoleTypes.Admin,
					RoleTypes.User
				};

				await _roleRepository.CreateGroupRoleAsync(roles);
		    }
		}
	}
}
