namespace WorkManagementSystem.Features.WorkArrived.AddWorkArrived
{
    public class Request
    {
        public string? WorkItemNumber { get; set; }
        public string? ItemId { get; set; }  // số
        public string? Notation { get; set; }  // ký hiệu link tới bảng setting
        public DateTime? DateIssued { get; set; }  // ngày ban hành  chính là ngày văn bản
        public string DocumentTypeKey { get; set; }  //  Loại văn bản link tới bảng chung setting
        public Guid? DepartmentId { get; set; }  // ĐƠn vị gửi
        public string Content { get; set; }   // Trích yếu
        public Guid LeadershipDirectId { get; set; }   // lãnh đạo phê duyệt
        public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
        public DateTime? Dealine { get; set; }  // Thời hạn xử lý
        public Guid UserCompile { get; set; }    // Người biên soạn
        public TransferTypeEnum TransferType { get; set; }  // hình thức vận chuyển
        public WorkArrivedStatus WorkArrivedStatus { get; set; }  // trạng thái của công văn
    }
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Content)
                 .NotEmpty().WithMessage("Nội dung trích yếu không được để trống!");
            // RuleFor(x => x.UserId).NotNull().WithMessage("Người phụ trách không được để trống");
        }
    }
    public class Response
    {
        public string Message => "Work arrived saved!";
        public string WorkItemId { get; set; }
    }
}
