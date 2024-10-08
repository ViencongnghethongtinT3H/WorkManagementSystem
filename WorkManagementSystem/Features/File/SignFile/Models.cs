namespace WorkManagementSystem.Features.File.SignFile
{
    public class Request
    {
        public Guid FileId { get; set; }
        public int TypeFile {  get; set; }
    }   

    public class Response
    {
        public string Output { get; set; }
    }
}
