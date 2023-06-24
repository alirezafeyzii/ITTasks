using ITTasks.DataLayer;
using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS.Sprints;
using Microsoft.EntityFrameworkCore;

namespace ITTasks.Repositories.Sprints
{
	public class SprintRepository : ISprintRepository
	{
		private readonly ITDbContext _dbContext;

		public SprintRepository(ITDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Sprint> CreateAsync(CreateSprintDto sprint, DateTime startDate, DateTime endDate)
		{
			var sprintFromDb = await _dbContext.Sprints.AddAsync(new Sprint
			{
				Title = sprint.Title,
				StartDate = startDate,
				EndDate = endDate,
				CreatedDate = DateTime.Now,
				UpdatedDate = DateTime.MinValue
			});

			await _dbContext.SaveChangesAsync();

			return sprintFromDb.Entity;
		}

		public async Task DeleteSprintAsync(Guid id)
		{
			var sprint = await _dbContext.Sprints.FirstOrDefaultAsync(x =>x.Id == id);
			_dbContext.Sprints.Remove(sprint);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<List<Sprint>> GetAllAsync()
		{
			return await _dbContext.Sprints.OrderByDescending(x => x.CreatedDate).ToListAsync();
		}

		public async Task<Sprint> GetByTitleAsync(string title)
		{
			var sprint = await _dbContext.Sprints.SingleOrDefaultAsync(x => x.Title.ToLower() == title.ToLower());
			if (sprint == null)
				return null;

			return sprint;
		}
	}
}
