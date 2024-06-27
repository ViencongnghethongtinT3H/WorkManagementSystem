namespace WorkManagementSystem.Features.WorkArrived.GetWorkArriveById
{
    public class Request
    {
        public Guid WorkDispatchId { get; set; }
    }
    public class WorkArriveDetailResponse
    {
        public string? WorkItemNumber { get; set; }
        public string? ItemId { get; set; }  // số
        public string? Notation { get; set; }  // ký hiệu link tới bảng setting
        public string? DateIssued { get; set; }  // ngày ban hành  chính là ngày văn bản
        public string DocumentTypeKey { get; set; }  //  Loại văn bản link tới bảng chung setting
        public Guid? DepartmentId { get; set; }  // ĐƠn vị gửi
        public string Content { get; set; }   // Trích yếu
        public Guid LeadershipDirectId { get; set; }   // lãnh đạo phê duyệt
        public string? LeadershipDirectName { get; set; }
        public string? DepartmentName { get; set; }
        public string? IndustryId { get; set; }    // Lĩnh vực  link tới bảng chung setting
        public string? IndustryName { get; set; }    // Lĩnh vực  link tới bảng chung setting
        public SettingEnum TypeSetting { get; set; }
        public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
        public string? Dealine { get; set; }  // Thời hạn xử lý
        public TransferTypeEnum TransferType { get; set; }  // hình thức vận chuyển
        public WorkArrivedStatus WorkArrivedStatus { get; set; }  // trạng thái của công văn
        public StepEnum Step { get; set; }
        public Guid UserConfirm { get; set; } // người xử lý của bước đấy
        public string? Note { get; set; }
        public List<ReceiveCompanyModel> ReceiveCompanys { get; set; }
    }
    public class ReceiveCompanyModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
        public Guid AccountReceiveId { get; set; }
    }
}
