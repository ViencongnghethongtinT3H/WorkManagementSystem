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
        public string DocumentTypeKey { get; set; }  //  Loại văn bản link tới bảng chung setting

        public string? DepartmentName { get; set; }   // Tên Cơ quan ban hành  
        public Guid? DepartmentId { get; set; }  //  cơ quan ban hành

        public string Content { get; set; }   // Trích yếu         
        public string? Subjective { get; set; }   // Chuyên đề   
        public string? KeyWord { get; set; }   // từ khoá 

        public string LeadershipDirectName { get; set; }   // Lãnh đạo chỉ đạo
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

        public string TransferType { get; set; }  // hình thức vận chuyển

        #endregion

        public WorkflowStatusEnum WorkflowStatus { get; set; }   // trạng thái của công vă

        // files
        public string FileName { get; set; }  // tên phòng ban
        public string? FileUrl { get; set; }  // tên phòng ban
        public string? FileExtension { get; set; }  // tên phòng ban

        public List<ReceiveCompanyModel> ReceiveCompanys { get; set; }
        public List<Notes> Notes { get; set; }   // Ý kiến xử lý
        public List<HistoryListModel> Histories { get; set; }  // lịch sử
    }
    public class ReceiveCompanyModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
        public Guid AccountReceiveId { get; set; }
    }

    public class Notes
    {
        public string DateNote { get; set; }   // ToFormatString("dd/MM/yyyy HH:mm"),
        public string UserName { get; set; }   // người thực hiện
        public string Note { get; set; }   // người thực hiện
        public string DeparmentName { get; set; }   // người thực hiện
    }

    public class HistoryListModel
    {
        public string ActionTime { get; set; }
        public string UserUpdated { get; set; }
        public string ActionContent { get; set; }
    }

}
