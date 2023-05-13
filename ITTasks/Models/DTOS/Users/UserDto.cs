namespace ITTasks.Models.DTOS.Users
{
    public class UserDto : BaseDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
