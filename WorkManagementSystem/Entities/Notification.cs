namespace WorkManagementSystem.Entities
{
    public class Notification : EntityBase
    {
        [MaxLength(100)]
        public Guid UserSend { get; set; }
        [MaxLength(100)]
        public Guid UserReceive { get; set; }
        public DateTime SendingTime { get; set; } = DateTime.Now;
        public DateTime? ReceivingTime { get; set; }
        [MaxLength(500)]
        public string Content { get; set; }
        [MaxLength(100)]
        public string? Url { get; set; }
        [MaxLength(100)]
        public string? NotificationType { get; set; }
        public bool IsRead { get; set; } = false;


    }
}
