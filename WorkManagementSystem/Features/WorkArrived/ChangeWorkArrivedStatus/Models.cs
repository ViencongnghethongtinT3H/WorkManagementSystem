namespace WorkManagementSystem.Features.WorkArrived.ChangeWorkArrivedStatus
{
    public class Request
    {
        public string Note { get; set; }
        public List<Guid> UserIds { get; set; }
        public Guid WorkArriveId { get; set; }
        public ActionType ActionType { get; set; }
        public Guid? UserId { get; set; }

    }
    public enum ActionType
    {
        [Description("Đã duyệt công văn")]
        Submited = 1,  // người xử lý đã duyệt công văn
        [Description("Đã huỷ công văn ")]
        Canceled = 2,   // người xử lý đã huỷ công văn 
        [Description("Đã trả lại công văn")]
        Return = 3,     // Người xử lý đã trả lại công văn
        [Description("Đang xử lý")]
        Proccessing = 4,  // từ chờ xử lý sang đang xử lý
        [Description("Đã lưu hồ sơ")]
        Save = 5, // luu ho so
        [Description("Đã chuyển người xử lý")]
        Swap = 7 // chuyen nguoi xu ly
    }
}
