namespace WorkManagementSystem.Features.WorkItem
{
    public class WorkItemModel
    {
        #region Step 1

        [MaxLength(100)]
        public string? ItemId { get; set; }  // số 
        [MaxLength(100)]
        public string? Notation { get; set; }  // ký hiệu
        public DateTime? DateIssued { get; set; }  // ngày ban hành
        //public DateTime DateArrival { get; set; }  // thời gian người nhận nhiệm vụ mở công văn này
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
        public Guid? CategoryId { get; set; }    // Lĩnh vực link tới bảng chung setting
        public ProcessingStatusEnum ProcessingStatus { get; set; }
        public StatusEnum Status { get; set; }

        #endregion
    }
}
