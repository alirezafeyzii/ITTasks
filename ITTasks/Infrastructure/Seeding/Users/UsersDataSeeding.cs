using ITTasks.DataLayer;
using ITTasks.DataLayer.Entities;
using ITTasks.Infrastructure.Utilities;
using ITTasks.Repositories.Roles;
using ITTasks.Repositories.Users;
using ITTasks.Statics.Entities.Roles;

namespace ITTasks.Infrastructure.Seeding.Users
{
	public class UsersDataSeeding
	{
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly ITDbContext _dbContext;

        public UsersDataSeeding(IUserRepository userRepository,
			ITDbContext dbContext,
			IRoleRepository roleRepository)
        {
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_dbContext = dbContext;
		}

		public async Task SeedUserAsync()
		{
			var userExists = await _userRepository.GetAllUsersAsync();

			var role = await _roleRepository.GetRoleByTypeAsync(RoleTypes.Admin);

			if (userExists.Count() < 1)
			{
				var user = new User
				{
					Id = Guid.NewGuid(),
					FullName = "ادمین",
					UserName = "administrator",
					NormalizedUserName = "ADMINISTRATOR",
					Email = "alireza.feizy22@gmail.com",
					NormalizedEmail = "ALIREZA.FEIZY22@GMAIL.COM",
					EmailConfirmed = true,
					IsActive = true,
					PhoneNumber = "09199638986",
					PhoneNumberConfirmed = false,
					PasswordHash = "Datis@123".Encrypt(),
					Token = "H7HnGBPNde2rvMimy4aFX0HRuL0NqwZWatk9Lm53xMJESrjrQ1yUP9RqrGvSKic7JJhQE9LUdXfERcEPVFMKGK1VuZPMahBZBFmi",
					CreatedTime = DateTime.Now,
					UpdatedTime = DateTime.MinValue,
					RoleId = role.Id
				};

				await _dbContext.Users.AddAsync(user);

				await _dbContext.SaveChangesAsync();
			}
		}
	}
}
