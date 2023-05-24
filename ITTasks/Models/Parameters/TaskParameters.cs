namespace ITTasks.Models.Parameters
{
	public class TaskParameters
	{
		public string SearchParameter { get; set; }
		public Guid UserId { get; set; }
		public Guid SprintId { get; set; }
		public int UnitId { get; set; }
		public long StartDate { get; set; }
		public long EndDate { get; set; }

		const int maxPageSize = 20;
		public int PageNumber { get; set; } = 1;

		private int _pageSize = 10;
		public int PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = (value > maxPageSize) ? maxPageSize : value;
			}
		}
	}
}
