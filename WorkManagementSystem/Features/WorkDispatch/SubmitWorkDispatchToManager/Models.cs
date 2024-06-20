namespace WorkManagementSystem.Features.WorkDispatch.SubmitWorkDispatchToManager
{
    public class Request
    {
        public Guid WorkDispatchId { get; set; }
        public Guid UserId { get; set; }  // Nguoi duyet hoac nguoi ky
        public bool IsSubmit { get; set; }   // true : trinh duyet, false: trinh ky
    }
}
