using ITTasks.DataLayer.Entities;
using ITTasks.Infrastructure.Helper;
using ITTasks.Models.DTOS.Sprints;
using ITTasks.Models.DTOS.Tasks.TasksType;
using ITTasks.Models.DTOS.Users;

namespace ITTasks.Models.DTOS.Tasks
{
    public class ITTaskDto : BaseDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public float HourAmount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public UserDto User { get; set; }
        public string PersianDate { get; set; }
        public ITTaskTypeDto TaskType { get; set; }
        public PageInfo PageInfo { get; set; }
<<<<<<< HEAD
        public int UnitId { get; set; }
        public string UnitName { get; set; }
    }
=======
		public SprintDto Sprint { get; set; }
	}
>>>>>>> 7f83baa764d4a37059694368f130beb7f3d30410
}
