namespace WorkManagementSystem.Entities.Enums;

public enum ProcessingStatusEnum
{
    [Description("Đang thực hiện")]
    Processing = 1,  
    [Description("Đã hoàn thành ")]
    Completed = 2,  
    [Description("Chưa vào sổ")]
    None = 3      
}
