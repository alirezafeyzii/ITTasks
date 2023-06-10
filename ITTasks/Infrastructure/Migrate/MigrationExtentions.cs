using ITTasks.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace ITTasks.Infrastructure.Migrate
{
	public static class MigrationExtentions
	{
		public static void UseMigration(this IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();

			var context = scope.ServiceProvider.GetRequiredService<ITDbContext>();

			context?.Database.Migrate();
		}
	}
}
