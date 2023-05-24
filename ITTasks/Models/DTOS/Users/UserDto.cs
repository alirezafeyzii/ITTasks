using ITTasks.DataLayer.Entities;

namespace ITTasks.Models.DTOS.Users
{
    public class UserDto : BaseDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public List<UserDto> Users { get; set; }

        public static implicit operator UserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                IsActive = user.IsActive,
                CreatedTime = user.CreatedTime,
                UpdatedTime = user.UpdatedTime,
            };
        }
    }
}
