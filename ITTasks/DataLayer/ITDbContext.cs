using ITTasks.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITTasks.DataLayer
{
    public class ITDbContext : DbContext
    {
        public ITDbContext(DbContextOptions options) : base(options) 
        { }

        public DbSet<ITTask> Tasks { get; set; }
        public DbSet<User> Users { get;set; }
        public DbSet<ITTaskType> TasksType { get; set; }
        public DbSet<Sprint> Sprints { get; set; }

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//    => optionsBuilder.UseSqlite(@"Data Source = ittask.db");
	}
}
