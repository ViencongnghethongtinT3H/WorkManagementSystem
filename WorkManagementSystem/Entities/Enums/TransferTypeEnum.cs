namespace WorkManagementSystem.Entities.Enums;

public enum TransferTypeEnum
{
    [Description("Email")]
    Email = 1,    // 
    [Description("Chuyển phát nhanh")]
    ExpressDelivery = 2,
    [Description("Bản cứng")]
    HardFile = 3
}
