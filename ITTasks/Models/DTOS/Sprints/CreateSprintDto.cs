namespace ITTasks.Models.DTOS.Sprints
{
	public class CreateSprintDto
	{
		public string Title { get; set; }
		public long StartDate { get; set; }
		public long EndDate { get; set; }
	}
}
