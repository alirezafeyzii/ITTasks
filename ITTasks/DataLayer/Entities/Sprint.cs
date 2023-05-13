namespace ITTasks.DataLayer.Entities
{
	public class Sprint
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
		public virtual ICollection<ITTask> Tasks { get; set; } = new List<ITTask>();
	}
}
