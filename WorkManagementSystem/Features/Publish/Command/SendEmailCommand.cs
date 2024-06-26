namespace WorkManagementSystem.Features.Publish.Command
{
    public class SendEmailCommand : ICommand<bool>
    {
        public string toEmail { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public List<string> FileNames { get; set; }

    }
}