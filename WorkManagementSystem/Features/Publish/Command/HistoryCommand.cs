namespace WorkManagementSystem.Features.Publish.Command
{
    public class HistoryCommand : ICommand<bool>
    {
        public Guid UserId { get; set; }
        public Guid IssueId { get; set; }
        public string ActionContent { get; set; }
    }
}
