﻿using ITTasks.DataLayer;
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
			if (sprint.Title == null)
				return null;

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

		public async Task<List<Sprint>> GetAllAsync()
		{
			var a = await _dbContext.Sprints.OrderByDescending(x => x.CreatedDate).ToListAsync();
			return a;
		}
	}
}
