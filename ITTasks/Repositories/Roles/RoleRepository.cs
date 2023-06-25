using ITTasks.DataLayer;
using ITTasks.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITTasks.Repositories.Roles
{
	public class RoleRepository : IRoleRepository
	{
		private readonly ITDbContext _dbContext;

		public RoleRepository(ITDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task CreateGroupRoleAsync(List<string> types)
		{
			var roles = new List<Role>();

			foreach (var type in types)
			{
				var roleExists = await GetRoleByTypeAsync(type);
				if (roleExists != null)
				{
					return;
				}
			}

			foreach (var type in types)
			{
				roles.Add(new Role
				{
					Type = type,
					IsActive = true,
					CreateDate = DateTime.Now,
					UpdateDate = DateTime.MinValue,
				});
			}

		    await _dbContext.ITRoles.AddRangeAsync(roles);

			await _dbContext.SaveChangesAsync();
		}

		public async Task<Role> CreateRoleAsync(string type)
		{
			var roleExists = await GetRoleByTypeAsync(type);
			if (roleExists != null)
				return null;

			var role = await _dbContext.ITRoles.AddAsync(new Role
			{
				Type = type,
				IsActive = true,
				CreateDate = DateTime.Now,
				UpdateDate = DateTime.MinValue,
			});

			await _dbContext.SaveChangesAsync();

			return role.Entity;
		}

		public async Task<List<Role>> GetAllRoles()
		{
			return await _dbContext.ITRoles.ToListAsync();
		}

		public async Task<Role> GetRoleByTypeAsync(string type)
		{
			var role = await _dbContext.ITRoles
				.SingleOrDefaultAsync(r => r.Type.ToLower() == type.ToLower());
			return role;
		}
	}
}
