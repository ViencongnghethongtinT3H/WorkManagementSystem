namespace WorkManagementSystem.Features.WorkArrived.ChangeWorkArrivedStatus
{
    public class Request
    {
        public string Note { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkArriveId { get; set; }
        public WorkArrivedStatus WorkArrivedStatus { get; set; }
        public ActionType ActionType { get; set; }
    }
    public enum ActionType
    {
        Submited = 1,  // người xử lý đã duyệt công văn
        Canceled = 2,   // người xử lý đã huỷ công văn 
        Return = 3,     // Người xử lý đã trả lại công văn
        Proccessing = 4,  // từ chờ xử lý sang đang xử lý
        complete = 5 // hoan thanh
    }
}
