namespace WorkManagementSystem.Features.Folder.CreateFolder;

public class Mapper : Mapper<Request, ResultModel<bool>, Entities.FileManagement>
{
    public override FileManagement ToEntity(Request r) => new()
    {
        UserId = r.UserId,
        Name = r.Name,
        ParentId = r.ParentId,
        FileManagementType = r.FileManagementType,
    };
}
