namespace WorkManagementSystem.Features.File.GetFolders;


public class Request
{
  public Guid? ParentId { get; set; }
  public Guid UserId { get; set; }

}
public class Response
{
  public FolderModel Folders { get; set; }
  public List<FileAttach>? Files { get; set; }

}

public class FolderModel
{
  public string Name { get; set; }
  public Guid? ParentId { get; set; }
  public Guid Id { get; set; }
  public Guid UserId { get; set; }

  public FileManagementType FileManagementType { get; set; }

}