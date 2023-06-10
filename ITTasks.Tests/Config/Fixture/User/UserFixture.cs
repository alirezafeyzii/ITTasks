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

		public UserFixture() 
		{
			userRepository = new UserRepository(dbContext);
			userService = new Services.Users.UserService(userRepository);
		}
	}
}
