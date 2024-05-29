namespace WorkManagementSystem.Features.File.GetFileAttachs
{
    public class Request
    {
        public Guid IssuesId { get; set; }
    }
    public class FileViewModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
