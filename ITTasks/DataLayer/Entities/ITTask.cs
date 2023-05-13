using System.ComponentModel.DataAnnotations.Schema;

namespace ITTasks.DataLayer.Entities
{
    public class ITTask
    {
        public Guid Id { get; set; }
        public float HourAmount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public User User { get; set; }

		[ForeignKey("ITTaskTypeId")]
		public Guid ITTaskTypeId { get; set; }
		public ITTaskType ITTaskType { get; set; }

		[ForeignKey("SprintId")]
		public Guid SprintId { get; set; }
		public Sprint Sprint { get; set; }
	}
}

