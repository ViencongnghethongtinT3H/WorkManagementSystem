namespace WorkManagementSystem.Entities.Enums
{
    public enum WorkflowStatusEnum
    {
        [Description("Chờ phát hành")]
        Waitting = 1,
        [Description("Đã phát hành ")]
        Release = 2,
        [Description("Đã huỷ")]
        Cancel = 3,
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
        [Description("Chờ vào sổ văn bản đến")]
        WaittingWorkArrived = 9,
    }
}
