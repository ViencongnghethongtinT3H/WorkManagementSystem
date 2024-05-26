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

    public enum NotificationWorkItemType
    {
        [Description("Chuyển công văn tới CV đến")]
        SendWorkItem = 1,
        [Description("Cập nhật ý kiến xử lý")]
        UpdateWorkItem = 2,
    }
}
