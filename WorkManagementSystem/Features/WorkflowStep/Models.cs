namespace WorkManagementSystem.Features.WorkStep
{
    public class Request
    {
        public Guid WorkflowId { get; set; }
        public StepEnum Step { get; set; }
        public Guid UserConfirm { get; set; } // người xử lý của bước đấy
        public string? Note { get; set; }
    }
    public class Response
    {
        public string Message => "Work fllow step saved!";
        public object result { get; set; }
    }
}
