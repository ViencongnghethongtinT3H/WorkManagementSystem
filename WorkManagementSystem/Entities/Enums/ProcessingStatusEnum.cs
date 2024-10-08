namespace WorkManagementSystem.Entities.Enums;

public enum ProcessingStatusEnum
{
    [Description("Chưa thực hiện")]
    Todo = 1,  
    [Description("Đang thực hiện")]
    Processing = 2,  
    [Description("Kiểm thử")]
    Test = 3,
    [Description("Đã hoàn thành")]
    Done = 4,
}
