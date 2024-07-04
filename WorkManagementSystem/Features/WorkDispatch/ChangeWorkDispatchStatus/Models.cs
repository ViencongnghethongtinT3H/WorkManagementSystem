namespace WorkManagementSystem.Features.WorkDispatch.ChangeWorkDispatchStatus;

public class Request
{
    public List<Guid> UserIds { get; set; }
    public Guid WorkFlowId {  get; set; }
    public ActionType ActionType { get; set; }       
    public string? Note { get; set; }      
}

public enum ActionType
{
    [Description("duyệt công văn")]
    Submited = 1,  // người xử lý đã duyệt công văn
    [Description("ký số công văn")]
    Signatured = 2,   // người xử lý đã ký số công văn
    [Description("huỷ công văn")]
    Canceled = 3,   // người xử lý đã huỷ công văn 
    [Description("trả lại công văn")]
    Return = 4,     // Người xử lý đã trả lại công văn
    [Description("đang xử lý")]
    Proccessing = 5,  // từ chờ xử lý sang đang xử lý
}