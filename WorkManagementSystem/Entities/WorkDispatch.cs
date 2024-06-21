namespace WorkManagementSystem.Entities;

// Luồng công văn đi
public class WorkDispatch : EntityBase
{
    #region Step 1
    public string? WorkItemNumber { get; set; }
    [MaxLength(100)]
    public string? ItemId { get; set; }  // số
    [MaxLength(100)]
    public string? Notation { get; set; }  // ký hiệu link tới bảng setting
    public DateTime? DateIssued { get; set; }  // ngày ban hành
    public string DocumentTypeKey { get; set; }  //  Loại văn bản link tới bảng chung setting
    public Guid? DepartmentId { get; set; }  //  cơ quan ban hành

    [MaxLength(1000)]
    [Required]
    public string Content { get; set; }   // Trích yếu         
    [MaxLength(1000)]
    public string? Subjective { get; set; }   // Chuyên đề   
    [MaxLength(1000)]
    public string? KeyWord { get; set; }   // từ khoá 
    public Guid LeadershipDirectId { get; set; }   // Lãnh đạo chỉ đạo
    public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
    #endregion

    #region ver 2

    public DateTime? Dealine { get; set; }  // Thời hạn xử lý
    public DateTime? EvictionTime { get; set; }  // Thời hạn thu hồi
    public DateTime? SignDay { get; set; }  // Ngày ký
    public Guid? UserSign { get; set; }    // Người ký 
    public Guid? UserCompile { get; set; }    // Người biên soạn
    public Guid? DepartmentCompile { get; set; }  //  đơn vị soạn thảo
    public string? IndustryId { get; set; }    // Lĩnh vực  link tới bảng chung setting
    public TransferTypeEnum TransferType { get; set; }  // hình thức vận chuyển

    #endregion

    public WorkflowStatusEnum WorkflowStatus { get; set; }   // trạng thái của công văn

    // Thêm phần người theo dõi khi tạo mới văn bản đi
    
}
