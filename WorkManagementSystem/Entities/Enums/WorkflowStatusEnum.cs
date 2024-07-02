namespace WorkManagementSystem.Entities.Enums
{
    public enum WorkflowStatusEnum
    {
        [Description("Chờ phát hành")]
        Waitting = 1,
        [Description("Đang xử lý")]
        Proccesing = 2,        
        [Description("Huỷ")]
        Cancel = 4,
        [Description("Trả lại")]
        ReceiveProccess = 5,
        [Description("Chờ vào sổ")]
        WaittingWorkArrived = 6,
        [Description("Hoàn thành")]
        Done = 9,
    }

    public enum UserWorkflowStatusEnum
    {
        [Description("Chờ xử lý")]
        Waitting = 1,
        [Description("Đang xử lý ")]
        Proccesing = 2,
        [Description("Hoàn thành")]
        Done = 3,
        [Description("Huỷ")]
        Cancel = 4,
        [Description("Trả lại")]
        ReceiveProccess = 5       
    }
}
