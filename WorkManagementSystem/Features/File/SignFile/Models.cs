namespace WorkManagementSystem.Features.File.SignFile
{
    public class Request
    {
        public Guid FileId { get; set; }
        public Guid WorkFollowId { get; set; }
    }   

    public class Response
    {
        public string Output { get; set; }
    }
}
