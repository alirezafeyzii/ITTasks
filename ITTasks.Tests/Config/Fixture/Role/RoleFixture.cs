using ITTasks.Repositories.Roles;
using ITTasks.Services.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTasks.Tests.Config.Fixture.Role
{
	public class RoleFixture : BaseFixture
	{
		public RoleRepository roleRepository;
		public ITTasks.Services.Roles.RoleService roleService;

        public RoleFixture()
        {
			roleRepository = new RoleRepository(dbContext);
			roleService = new ITTasks.Services.Roles.RoleService(roleRepository);
        }
    }
}
