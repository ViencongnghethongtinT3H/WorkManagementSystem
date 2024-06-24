namespace WorkManagementSystem.Entities;

// Luồng công văn đến
public class WorkArrived : EntityBase
{
    public string? WorkItemNumber { get; set; }
    [MaxLength(100)]
    public string? ItemId { get; set; }  // số
    [MaxLength(100)]
    public string? Notation { get; set; }  // ký hiệu link tới bảng setting
    public DateTime? DateIssued { get; set; }  // ngày ban hành  chính là ngày văn bản
    public string DocumentTypeKey { get; set; }  //  Loại văn bản link tới bảng chung setting
    public Guid? DepartmentId { get; set; }  // ĐƠn vị gửi
    [MaxLength(1000)]
    [Required]
    public string Content { get; set; }   // Trích yếu
    public Guid LeadershipDirectId { get; set; }   // lãnh đạo phê duyệt
    public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
    public DateTime? Dealine { get; set; }  // Thời hạn xử lý
    public TransferTypeEnum TransferType { get; set; }  // hình thức vận chuyển
    public WorkArrivedStatus WorkArrivedStatus { get; set; }  // trạng thái của công văn
    
}
