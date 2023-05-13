namespace ITTasks.Models.DTOS.Tasks.TasksType
{
	public class ITTaskTypeDto : BaseDTO
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public DateTime CreateTime { get; set; }
		public DateTime UpdateTime { get; set; }
	}
}
