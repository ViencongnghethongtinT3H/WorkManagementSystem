namespace WorkManagementSystem.Features.Folder.GetListFolder;


public class Request
{
  public Guid? ParentId { get; set; }
  public Guid UserId { get; set; }

}
public class Response
{
    public FolderModel Folder { get; set; }
    public List<FileAttach> Files { get; set; }
    public List<Response> Children { get; set; }
}

public class FolderModel
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
    public FileManagementType FileManagementType { get; set; }
}
