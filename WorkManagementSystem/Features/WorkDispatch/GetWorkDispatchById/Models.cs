namespace WorkManagementSystem.Features.WorkDispatch.GetWorkDispatchById
{
    public class Request
    {
        public Guid WorkDispatchId { get; set; }
    }
    public class WorkDispatchDetailResponse
    {
        #region Step 1
        public string? WorkItemNumber { get; set; }
        public Guid WorkDispatchId { get; set; }
        public string? UserIdCreated { get; set; }
        public string? ItemId { get; set; }  // số

        public string? Notation { get; set; }  // ký hiệu link tới bảng setting
        public string? DateIssued { get; set; }  // ngày ban hành
        public string DocumentTypeKey { get; set; } = string.Empty;  //  Loại văn bản link tới bảng chung setting

        public string? DepartmentName { get; set; }   // Tên Cơ quan ban hành  
        public Guid? DepartmentId { get; set; }  //  cơ quan ban hành

        public string Content { get; set; } = string.Empty;   // Trích yếu         
        public string? Subjective { get; set; }   // Chuyên đề   
        public string? KeyWord { get; set; }   // từ khoá 

        public string LeadershipDirectName { get; set; } = string.Empty;  // Lãnh đạo chỉ đạo
        public Guid LeadershipDirectId { get; set; }   // Lãnh đạo chỉ đạo
        public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
        #endregion

        #region ver 2

        public string? Dealine { get; set; }  // Thời hạn xử lý
        public string? EvictionTime { get; set; }  // Thời hạn thu hồi
        public string? SignDay { get; set; }  // Ngày ký
        public Guid? UserSign { get; set; }    // Người ký 
        public Guid? UserCompile { get; set; }    // Người biên soạn
        public Guid? DepartmentCompile { get; set; }  //  đơn vị soạn thảo
        public string? IndustryId { get; set; }    // Lĩnh vực  link tới bảng chung setting
        public string? IndustryName { get; set; }    // Lĩnh vực  link tới bảng chung setting

        public string TransferType { get; set; } = string.Empty;  // hình thức vận chuyển

        #endregion

        public WorkflowStatusEnum WorkflowStatus { get; set; }   // trạng thái của công vă


        public List<FileModel> Files { get; set; } = new List<FileModel>();
        public List<ReceiveCompanyModel> ReceiveCompanys { get; set; } = new List<ReceiveCompanyModel>();
        public List<Notes> Notes { get; set; } = new List<Notes>();  // Ý kiến xử lý
        public List<HistoryListModel> Histories { get; set; } = new List<HistoryListModel>();  // lịch sử
    }
    public class Notes
    { 
        public Guid Id { get; set; }
        public Guid WorkFlow { get; set; }
        public string Note { get; set; } = string.Empty;
        public string Created { get; set; } = string.Empty;
        public string DeparmentName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

    }
    public class ReceiveCompanyModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
        public Guid? AccountReceiveId { get; set; }
    }

    public class HistoryListModel
    {
        public string ActionTime { get; set; } = string.Empty;
        public string UserUpdated { get; set; } = string.Empty;
        public string ActionContent { get; set; } = string.Empty;
    }
    public class FileModel 
    {
        // files
        public string FileName { get; set; } = string.Empty; // tên phòng ban
        public string? FileUrl { get; set; }  // tên phòng ban
        public string? FileExtension { get; set; }  // tên phòng ban
        public Guid FileId { get; set; }
        public StatusEnum Status { get; set; }

    }


}
