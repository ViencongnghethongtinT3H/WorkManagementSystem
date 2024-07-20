namespace WorkManagementSystem.Features.Publish.Command
{
    public class GetNotationWorkDispatchCommand : ICommand<string>
    {
        public Guid WorkDispatchId { get; set; }
    }
}
