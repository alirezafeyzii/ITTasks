namespace ITTasks.DataLayer.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set;}
        public virtual ICollection<ITTask> Tasks { get; set; } = new List<ITTask>();
    }
}
