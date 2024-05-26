namespace WorkManagementSystem.Features.WorkItem.GetWorkItemById;

public class Request
{
    public Guid WorkId { get; set; }
}
public class WorkItemDetailResponse
{
    public string WorkItemNumber { get; set; }
    public Guid WorkItemId { get; set; }
    public Guid? UserId { get; set; }   // người chủ trì công văn   
    public string Notation { get; set; }  // số, ký hiệu
    public string? DateIssued { get; set; }  // ngày ban hành
    public string Content { get; set; }   // Trích yếu         
    public Guid LeadershipDirectId { get; set; }   // Tên lãnh đạo chỉ đạo      
    public string? Subjective { get; set; }   // Chuyên đề  
    public Guid? DepartmentId { get; set; }   // Cơ quan ban hành  
    public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
    public ProcessingStatusEnum ProcessingStatus { get; set; }

    #region Step 2

    public string? Dealine { get; set; }  // Thời hạn xử lý
    public string? EvictionTime { get; set; }  // Thời hạn thu hồi
    public Guid? IndustryId { get; set; }    // Lĩnh vực  link tới bảng chung setting

    #endregion

    public List<Implemention> Implementions { get; set; }
    public List<HistoryListModel> Histories { get; set; }

}
public class Implemention
{
    public Guid UserReceiveId { get; set; }  // Id của nhân viên thực hiện
    public string UserName { get; set; }  // Id của nhân viên thực hiện
    public Guid? DepartmentReceiveId { get; set; }  //  cơ quan nhận công văn, với trường hợp đây là công văn
    public string DepartmentReceiveName { get; set; }  //  cơ quan nhận công văn, với trường hợp đây là công văn
    public string? Note { get; set; }  // ý kiến xử lý
    public string CreatedDate { get; set; }  // ý kiến xử lý
}
public class HistoryListModel
{
    public string ActionTime { get; set; }
    public string UserUpdated { get; set; }
    public string ActionContent { get; set; }
}
