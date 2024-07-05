namespace WorkManagementSystem.Entities;

public class FileAttach: EntityBase
{
    public Guid? IssuesId { get; set; }   // Link tới công văn hay nhiệm vụ
    [MaxLength(1000)]
    public string FileName { get; set; }  // tên phòng ban
    [MaxLength(1000)]
    public string FileUrl { get; set; }  // tên phòng ban
    public Guid RefId { get; set; }  // Link tới bảng file manager
    [MaxLength(50)]
    public string? FileExtension { get; set; }  // tên phòng ban
}
