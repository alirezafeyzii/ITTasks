using ITTasks.DataLayer;
using ITTasks.Models.DTOS.Users;
using ITTasks.Models.Errors;
using ITTasks.Repositories.Users;
using ITTasks.Services.Users;
using ITTasks.Tests.Config.Fixture;
using ITTasks.Tests.Config.Fixture.User;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTasks.Tests.UserService
{
	public class UserServiceTest : IClassFixture<UserFixture>
	{
		public ITTasks.Services.Users.UserService userService;

		public UserServiceTest(UserFixture fixture)
		{
			userService = fixture.userService;
		}

		[Fact]
		public async Task CreateUserTest()
		{
			var user = new CreateUserDto
			{
				FullName = "TestName",
			};

			var userFromService = await userService.CreateUserAsync(user);

			Assert.NotNull(userFromService);
			Assert.NotNull(userFromService.FullName);

			userFromService.FullName.ShouldBe("TestName");
			userFromService.Id.ShouldNotBe(Guid.Empty);
			userFromService.IsActive.ShouldBe(false);
			userFromService.CreatedTime.ShouldNotBe(DateTime.MinValue);
			userFromService.UpdatedTime.ShouldBe(DateTime.MinValue);
			userFromService.ErrorCode.ShouldBe((int)ErrorCodes.NoError);
			userFromService.ErrorMessage.ShouldBe(ErrorMessages.NoError);
		}

		[Fact]
		public async Task GetUserByIdTest()
		{
			var user = new CreateUserDto
			{
				FullName = "TestName",
			};

			var userAfterCreate = await userService.CreateUserAsync(user);

			var userFromService = await userService.GetUserByIdAsync(userAfterCreate.Id);

			Assert.NotNull(userFromService);
			Assert.NotNull(userFromService.FullName);

			userFromService.FullName.ShouldBe("TestName");
			userFromService.Id.ShouldNotBe(Guid.Empty);
			userFromService.IsActive.ShouldBe(false);
			userFromService.CreatedTime.ShouldNotBe(DateTime.MinValue);
			userFromService.UpdatedTime.ShouldBe(DateTime.MinValue);
			userFromService.ErrorCode.ShouldBe((int)ErrorCodes.NoError);
			userFromService.ErrorMessage.ShouldBe(ErrorMessages.NoError);
		}

		[Fact]
		public async Task UpdateUserTest()
		{
			var user = new CreateUserDto
			{
				FullName = "TestName",
			};

			var userFromService = await userService.CreateUserAsync(user);

			Assert.NotNull(userFromService);
			Assert.NotNull(userFromService.FullName);

			userFromService.FullName.ShouldBe("TestName");
			userFromService.Id.ShouldNotBe(Guid.Empty);
			userFromService.IsActive.ShouldBe(false);
			userFromService.CreatedTime.ShouldNotBe(DateTime.MinValue);
			userFromService.UpdatedTime.ShouldBe(DateTime.MinValue);
			userFromService.ErrorCode.ShouldBe((int)ErrorCodes.NoError);
			userFromService.ErrorMessage.ShouldBe(ErrorMessages.NoError);

			var userForUpdate = new UserDto
			{
				Id = userFromService.Id,
				FullName = "UpdatedTestName"
			};

			var userAfterUpdated = await userService.UpdatUserAsync(userForUpdate);

			Assert.NotNull(userAfterUpdated);
			Assert.NotNull(userAfterUpdated.FullName);

			userAfterUpdated.FullName.ShouldBe("UpdatedTestName");
			userAfterUpdated.Id.ShouldNotBe(Guid.Empty);
			userAfterUpdated.IsActive.ShouldBe(false);
			userAfterUpdated.CreatedTime.ShouldNotBe(DateTime.MinValue);
			userAfterUpdated.UpdatedTime.ShouldNotBe(DateTime.MinValue);
			userAfterUpdated.ErrorCode.ShouldBe((int)ErrorCodes.NoError);
			userAfterUpdated.ErrorMessage.ShouldBe(ErrorMessages.NoError);
		}
	}
}
