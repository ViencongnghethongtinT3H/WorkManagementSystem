namespace WorkManagementSystem.Entities.Enums;

public enum ProcessingStatusEnum
{
    [Description("Đang thực hiện")]
    Processing = 1,  
    [Description("Đã hoàn thành ")]
    Completed = 2,  
    [Description("Chưa vào sổ")]
    None = 3,
    [Description("Đã chuyển xử lý")]
    ReceiveProccess = 4,
}
