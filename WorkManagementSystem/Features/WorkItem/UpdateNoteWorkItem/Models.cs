namespace WorkManagementSystem.Features.WorkItem.UpdateNoteWorkItem
{
    public class Request
    {
        public Guid WorkItemId { get; set; }
        public Guid UserReceiveId {  get; set; }
        public Guid DepartmentReceiveId {  get; set; }
        public string Note { get; set; }
        public List<Guid>? FileAttachIds { get; set; }
    }
}
