using ITTasks.DataLayer;
using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS.Tasks.TasksType;
using ITTasks.Repositories.Tasks.TasksType;

namespace ITTasks.Infrastructure.Seeding.TaskTypes
{
	public class TaskTypesSeeding
	{
		private readonly ITaskTypeRepository _taskTypeRepository;

		public TaskTypesSeeding(ITaskTypeRepository taskTypeRepository)
        {
			_taskTypeRepository = taskTypeRepository;
		}

		public async Task SeedTaskTypeAsync()
		{
			var taskTypeExists = await _taskTypeRepository.GetAllAsync();
			if(taskTypeExists.Count() < 1)
			{
				var taskTypes = new List<ITTaskTypeCreateDto>
				{
					new ITTaskTypeCreateDto
					{
						Title = "پرینتر و اسکنر"
					},
					new ITTaskTypeCreateDto
					{
						Title = "مودم"
					},
					new ITTaskTypeCreateDto
					{
						Title = "ERP"
					},
					new ITTaskTypeCreateDto
					{
						Title = "VOIP"
					},
					new ITTaskTypeCreateDto
					{
						Title = "چینه"
					},
					new ITTaskTypeCreateDto
					{
						Title = "ساخت کاربر"
					},
					new ITTaskTypeCreateDto
					{
						Title = "VPN"
					},
					new ITTaskTypeCreateDto
					{
						Title = "مشکل ایمیل"
					},
					new ITTaskTypeCreateDto
					{
						Title = "تلفن"
					},new ITTaskTypeCreateDto
					{
						Title = "سرور"
					},
					new ITTaskTypeCreateDto
					{
						Title = "مشکل اکسل"
					},
					new ITTaskTypeCreateDto
					{
						Title = "مشکل اینترنت"
					},
					new ITTaskTypeCreateDto
					{
						Title = "جلسه"
					},
					new ITTaskTypeCreateDto
					{
						Title = "اسمبل کردن سیستم"
					},
					new ITTaskTypeCreateDto
					{
						Title = "نصب ویندوز"
					},
					new ITTaskTypeCreateDto
					{
						Title = "سایر"
					},
				};

				foreach (var taskType in taskTypes)
				{
					await _taskTypeRepository.CreateAsync(taskType);
				}
			}
		}

	}
}
