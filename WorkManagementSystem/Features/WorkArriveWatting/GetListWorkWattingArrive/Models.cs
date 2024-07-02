namespace WorkManagementSystem.Features.WorkArriveWatting.GetListWorkWattingArrive
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
        #region Step 1
        public string? WorkItemNumber { get; set; }
        public Guid WorkArriveWattingId { get; set; }
        public string? ItemId { get; set; }  // số

        public string? Notation { get; set; }  // ký hiệu link tới bảng setting
        public string? DateIssued { get; set; }  // ngày ban hành
        public string DocumentTypeKey { get; set; }  //  Loại văn bản link tới bảng chung setting

        public string? DepartmentName { get; set; }   // Tên Cơ quan ban hành  

        public string Content { get; set; }   // Trích yếu         
        public string? Subjective { get; set; }   // Chuyên đề   
        public string? KeyWord { get; set; }   // từ khoá 

        public string LeadershipDirectName { get; set; }   // Lãnh đạo chỉ đạo
        [JsonIgnore]
        public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
        #endregion

        #region ver 2

        public string? Dealine { get; set; }  // Thời hạn xử lý
        public string? EvictionTime { get; set; }  // Thời hạn thu hồi
        public string? SignDay { get; set; }  // Ngày ký
        public Guid? UserSign { get; set; }    // Người ký 
        public Guid? UserCompile { get; set; }    // Người biên soạn
        public Guid? DepartmentCompile { get; set; }  //  đơn vị soạn thảo
        public string? IndustryName { get; set; }    // Lĩnh vực  link tới bảng chung setting
        [JsonIgnore]
        public string TransferType { get; set; }  // hình thức vận chuyển

        #endregion

        [JsonIgnore]
        public string WorkflowStatus { get; set; }   // trạng thái của công văn

        public DateTime Created { get; set; }
    }

}
