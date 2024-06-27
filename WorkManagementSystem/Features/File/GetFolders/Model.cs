namespace WorkManagementSystem.Features.File.GetFolders;


public class Request
{
  public Guid? ParentId { get; set; }
}
public class Response
{
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public Guid Id { get; set; }
    public FileManagementType FileManagementType { get; set; }
}
