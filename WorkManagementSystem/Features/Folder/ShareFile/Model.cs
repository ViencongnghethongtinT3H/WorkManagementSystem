namespace WorkManagementSystem.Features.Folder.ShareFile;



public class Request
{
  public Guid FolderId { get; set; }
  public Guid FileId { get; set; }

}
public class Response
{
    public Guid Id { get; set; }
}

