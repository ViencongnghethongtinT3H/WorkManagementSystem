namespace WorkManagementSystem.Features.Publish.Command
{
    public class GetSubjectWorkDispatchCommand : ICommand<string>
    {
        public Guid WorkDispatchId { get; set; }
    }
}
