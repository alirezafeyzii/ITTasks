using ITTasks.DataLayer.Entities;
using ITTasks.Models.DTOS.Users;

namespace ITTasks.Models.DTOS.Sprints
{
	public class SprintDto : BaseDTO
	{
		public Guid Id	 { get; set; }
		public string Title { get; set; }
		public long StartDate { get; set; }
		public long EndDate { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string StringStartDate { get;set; }
		public string StringEndDate { get; set; }

		
	}
}
