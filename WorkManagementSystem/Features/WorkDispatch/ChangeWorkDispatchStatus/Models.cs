namespace WorkManagementSystem.Features.WorkDispatch.ChangeWorkDispatchStatus;

public class Request
{
    public Guid UserId { get; set; }
    public Guid WorkFlowId {  get; set; }
    public ActionType ActionType { get; set; }       
    public string? Note { get; set; }      
}

public enum ActionType
{   
    Submited = 1,  // người xử lý đã duyệt công văn
    Signatured = 2,   // người xử lý đã ký số công văn
    Canceled = 3,   // người xử lý đã huỷ công văn 
    Return = 4,     // Người xử lý đã trả lại công văn
    Proccessing = 5,  // từ chờ xử lý sang đang xử lý
}