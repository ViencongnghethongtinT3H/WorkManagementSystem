namespace WorkManagementSystem.Features.Publish.Command
{
    public class NoteCommand : ICommand<bool>
    {
        public Guid UserId { get; set; }
        public Guid WorkFlow { get; set; }
        [MaxLength(250)]
        public string Notes { get; set; }
    }
}
