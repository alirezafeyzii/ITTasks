using ITTasks.DataLayer;
using ITTasks.Repositories.Users;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTasks.Tests.Config.Fixture
{
	public class BaseFixture
	{
		public ITDbContext dbContext;

		public BaseFixture()
		{
			string connectionStrings = "Data Source=ittask.db";

			var connection = new SqliteConnection(connectionStrings);

			connection.Open();

			var options = new DbContextOptionsBuilder<ITDbContext>().UseSqlite(connection).Options;

			dbContext = new ITDbContext(options);

			dbContext.Database.EnsureCreated();
		}
	}
}
