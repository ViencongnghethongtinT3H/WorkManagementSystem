namespace WorkManagementSystem.Features.WorkArrived.GetListWorkDispatchWattingArrive
{
    public class Request
    {
        [FromHeader]
        public string Query { get; set; }
    }

    public class InputRequest : IListQuery<Response>
    {
        public List<FilterModel> Filters { get; init; } = new();
        public List<SortModel> Sorts { get; init; } = new();
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public bool IsPagingEnabled { get; init; } = false;
        public Guid UserId { get; set; }
    }

    public class Response : QueryListResponse<WorkArriveResponse>
    {

    }
    public class WorkArriveResponse
    {
        public string? WorkItemNumber { get; set; }
        [MaxLength(100)]
        public string? ItemId { get; set; }  // số
        [MaxLength(100)]
        public string? Notation { get; set; }  // ký hiệu link tới bảng setting
        public string? DateIssued { get; set; }  // ngày ban hành  chính là ngày văn bản
        public string DocumentTypeKey { get; set; }  //  Loại văn bản link tới bảng chung setting
        public Guid? DepartmentId { get; set; }  // ĐƠn vị gửi
        [MaxLength(1000)]
        [Required]
        public string Content { get; set; }   // Trích yếu
        public Guid LeadershipDirectId { get; set; }   // lãnh đạo phê duyệt
        public string Priority { get; set; }  // Độ khẩn cấp
        public string? Dealine { get; set; }  // Thời hạn xử lý
        public TransferTypeEnum TransferType { get; set; }  // hình thức vận chuyển
        public string WorkArrivedStatus { get; set; }  // trạng thái của công văn
    }

}
