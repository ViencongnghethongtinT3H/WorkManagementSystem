namespace WorkManagementSystem.Entities.Enums
{
    public enum FileManagementType
    {
        [Description("Thư mục chia sẻ")]
        Share = 1,
        [Description("Số hoá")]
        Digitization = 2,
        [Description("Công văn")]
        WorkItem = 3,
        [Description("Nhiệm vụ")]
        Document = 4,
    }
}
