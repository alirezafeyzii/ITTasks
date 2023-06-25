using ITTasks.Repositories.Roles;
using ITTasks.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTasks.Tests.Config.Fixture.User
{
	public class UserFixture : BaseFixture
	{
		private UserRepository userRepository;
		public ITTasks.Services.Users.UserService userService;
		public RoleRepository roleRepository;

		public UserFixture() 
		{
			userRepository = new UserRepository(dbContext, roleRepository);
			userService = new Services.Users.UserService(userRepository);
		}
	}
}
