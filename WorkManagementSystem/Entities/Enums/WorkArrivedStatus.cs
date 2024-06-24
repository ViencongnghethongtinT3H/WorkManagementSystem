namespace WorkManagementSystem.Entities.Enums
{
    public enum WorkArrivedStatus
    {
        [Description("Chờ vào sổ")]
        Waitting = 1,
        [Description("Chờ phát hành")]
        WaittingRelease = 2,
        [Description("Đã huỷ")]
        Proccesing = 3,
        [Description("Trả về")]
        ReceiveProccess = 4,
        [Description("Trình duyệt")]
        Submit = 5,
        [Description("Trình ký")]
        Signarture = 6,
        [Description("Đã duyệt")]
        Submited = 7,
        [Description("Đã ký")]
        Signartured = 8,
    }
}
