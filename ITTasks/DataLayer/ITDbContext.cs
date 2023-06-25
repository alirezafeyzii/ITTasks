using ITTasks.DataLayer.Entities;
using ITTasks.Statics.Entities.Roles;
using Microsoft.EntityFrameworkCore;

namespace ITTasks.DataLayer
{
    public class ITDbContext : DbContext
    {
        public ITDbContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<ITTask> Tasks { get; set; }
        public DbSet<User> Users { get;set; }
        public DbSet<ITTaskType> TasksType { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<Role> ITRoles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
    //        modelBuilder.Entity<Role>()
    //            .HasData(new Role
    //            {
    //                Id = Guid.NewGuid(),
    //                Type = RoleTypes.Admin,
    //                CreateDate = DateTime.Now,
    //                UpdateDate = DateTime.MinValue,
    //                IsActive = true,
    //            },
    //            new Role
    //            {
    //                Id = Guid.NewGuid(),
				//	Type = RoleTypes.User,
				//	CreateDate = DateTime.Now,
				//	UpdateDate = DateTime.MinValue,
				//	IsActive = true,
				//});
		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//    => optionsBuilder.UseSqlite(@"Data Source = ittask.db");
	}
}
