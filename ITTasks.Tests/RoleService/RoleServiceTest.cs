using ITTasks.Services.Roles;
using ITTasks.Tests.Config.Fixture.Role;
using ITTasks.Tests.Config.Fixture.User;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTasks.Tests.RoleService
{
	public class RoleServiceTest : IClassFixture<RoleFixture>
	{
		public ITTasks.Services.Roles.RoleService roleService;

        public RoleServiceTest(RoleFixture fixture)
        {
            roleService = fixture.roleService;
        }

        [Fact]
        public async Task CreateRoleTest()
        {
            var roleType = "user";

            var roleAfterCreate = await roleService.CreateRole(roleType);

            Assert.NotNull(roleAfterCreate);
            roleAfterCreate.Type.ShouldBe(roleType);
            roleAfterCreate.Id.ShouldNotBe(Guid.Empty);
            roleAfterCreate.CreateDate.ShouldNotBe(DateTime.MinValue);
			roleAfterCreate.UpdateDate.ShouldBe(DateTime.MinValue);
            roleAfterCreate.IsActive.ShouldBeTrue();
		}
    }
}
