namespace WorkManagementSystem.Features.File.GetFileByFolderId;


public class Request
{
  public Guid? FolderId { get; set; }
}
public class Response
{
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public Guid? IssuesId { get; set; }   // Link tới công văn hay nhiệm vụ
        public Guid RefId { get; set; }  // Link tới bảng file manager
        public string? FileExtension { get; set; }  // tên phòng ban

}
