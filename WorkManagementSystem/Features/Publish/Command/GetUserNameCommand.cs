namespace WorkManagementSystem.Features.Publish.Command
{
    public class GetUserNameCommand : ICommand<string>
    {
        public Guid UserId { get; set; }
    }
}
