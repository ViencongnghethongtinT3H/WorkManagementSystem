namespace WorkManagementSystem.Entities.Enums
{
    public enum NotificationType
    {
        [Description ("Công văn")]
        WorkItem = 1,
        [Description("Nhiệm vụ")]
        Task = 2,
        [Description("Thông báo")] 
        Message = 3,
        [Description("Email")]
        Email = 4
    }
}
