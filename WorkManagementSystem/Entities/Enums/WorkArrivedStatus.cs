namespace WorkManagementSystem.Entities.Enums
{
    public enum WorkArrivedStatus
    {
        [Description("Chờ phát hành")]
        Waitting = 1,
        [Description("Đang xử lý")]
        Proccesing = 2,
        [Description("Huỷ")]
        Cancel = 4,
        [Description("Trả lại")]
        ReceiveProccess = 5,
        [Description("Chờ vào sổ văn bản đến")]
        WaittingWorkArrived = 6,
        [Description("Hoàn thành")]
        Complete = 7,
    }

    public enum WorkArrivedProcedureStatus
    {
        [Description("Theo quy trình")]
        Flower = 1,
        [Description("Đang xử lý")]
        NotFlower = 2,
    }
}
