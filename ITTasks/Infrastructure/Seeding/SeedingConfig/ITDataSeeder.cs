using ITTasks.Infrastructure.Seeding.Roles;
using ITTasks.Infrastructure.Seeding.TaskTypes;
using ITTasks.Infrastructure.Seeding.Users;

namespace ITTasks.Infrastructure.Seeding.SeedingConfig
{
	public static class ITDataSeeder
	{
		public static void UseRoleDataSeed(this IHost app)
		{
			var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

			using (var scope = scopedFactory.CreateScope())
			{
				var service = scope.ServiceProvider.GetService<RolesDataSeeding>();
				service.SeedRoleAsync();
			}
		}

		public static void UseUserDataSeed(this IHost app)
		{
			var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

			using (var scope = scopedFactory.CreateScope())
			{
				var service = scope.ServiceProvider.GetService<UsersDataSeeding>();
				service.SeedUserAsync();
			}
		}

		public static void UseTaskTypesDataSeed(this IHost app)
		{
			var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

			using (var scope = scopedFactory.CreateScope())
			{
				var service = scope.ServiceProvider.GetService<TaskTypesSeeding>();
				service.SeedTaskTypeAsync();
			}
		}
	}
}
