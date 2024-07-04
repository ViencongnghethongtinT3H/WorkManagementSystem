namespace WorkManagementSystem.Features.Publish.Command
{
    public class SignFileCommand:  ICommand<string>
    {
        public string FileUrl { get; set; } 
        public string FileName { get; set; } 
    }
}
