namespace WorkManagementSystem.Features.TaskDetail.GetTaskDetailById
{
    public class Request
    {
        public Guid TaskId { get; set; }
    }


    public class TaskDetailResponse
    {
        public Guid Id { get; set; }
      //  public Guid? WorkItemId { get; set; }    // Link tới luồng công văn 
        //public Guid DepartmentSentId { get; set; }    // Đơn vị giao nhiệm vụ
        //public string DepartmentSentName { get; set; }    // tên Đơn vị giao nhiệm vụ
        public Guid DepartmentReceiveId { get; set; }    // Đơn vị nhận nhiệm vụ
        public string DepartmentReceiveName { get; set; }    // tên Đơn vị nhận nhiệm vụ
        public string Content { get; set; }   // nội dung công việc    
        public string Notation { get; set; }   // nội dung công việc    
        [MaxLength(1000)]
        public string? KeyWord { get; set; }   // từ khoá 
        public string? Dealine { get; set; }  // Thời hạn xử lý       
        public ProcessingStatusEnum ProcessingStatus { get; set; }  // trạng thái của nhiệm vụ
        public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
        public bool IsPeriodical { get; set; }  // Có định kỳ hay ko
        public PeriodicalEnum Periodical { get; set; }  // định kỳ theo tháng, quý, năm
        public string DealinePeriodical { get; set; }  // thời gian kết thúc xử lý định kỳ
        public Guid UserCreateTaskId { get; set; }    // Người nhập văn bản
        public string UserNameCreateTask { get; set; }    // Người nhập văn bản
        public Guid? LeadershipDirectId { get; set; }   // Lãnh đạo chỉ đạo
        public string LeadershipDirectName { get; set; }    // tên lãnh đạo chỉ đạo


        public List<Implemention> Implementions { get; set; }
        public List<HistoryListModel> Histories { get; set; }

    }
    public class Implemention
    {
        public Guid UserReceiveId { get; set; }  // Id của nhân viên thực hiện
        public string UserName { get; set; }  // Id của nhân viên thực hiện
        public string? Note { get; set; }  // ý kiến xử lý
        public string CreatedDate { get; set; }  // ý kiến xử lý
        public int ProgressValue { get; set; }  // ý kiến xử lý
    }
    public class HistoryListModel
    {
        public string ActionTime { get; set; } 
        public string UserUpdated { get; set; }
        public string ActionContent { get; set; }
    }

}
