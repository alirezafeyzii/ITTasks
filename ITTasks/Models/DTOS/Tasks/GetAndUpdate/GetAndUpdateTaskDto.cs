using ITTasks.Models.DTOS.Sprints;
using ITTasks.Models.DTOS.Tasks.TasksType;
using ITTasks.Models.DTOS.Users;

namespace ITTasks.Models.DTOS.Tasks.GetAndUpdate
{
	public class GetAndUpdateTaskDto
	{
		public ITTaskUpdateDto Task { get; set; }
		public List<ITTaskTypeDto> TaskTypes { get; set; }
		public List<UserDto> Users { get; set; }
		public List<SprintDto> Sprints { get; set; }
		public UserDto User { get; set; }
		public SprintDto Sprint { get; set; }
		public ITTaskTypeDto TaskType { get; set; }
	}
}
