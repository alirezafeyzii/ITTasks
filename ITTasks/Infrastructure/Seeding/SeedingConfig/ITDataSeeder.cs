using ITTasks.Infrastructure.Seeding.Roles;

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
				service.SeedingRoleAsync();
			}
		}
	}
}
