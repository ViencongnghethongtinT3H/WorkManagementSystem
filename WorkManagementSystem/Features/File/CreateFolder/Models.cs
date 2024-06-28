namespace WorkManagementSystem.Features.File.CreateFolder;

public class Request
{
    public Guid UserId { get; set; }

    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public FileManagementType FileManagementType { get; set; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Name)
             .NotEmpty().WithMessage("Tên không được để trống");
        RuleFor(x => x.UserId)
       .NotEmpty().WithMessage("Người dùng không được để trống");
        RuleFor(x => x.FileManagementType)
              .NotEmpty().WithMessage("Loại thư mục không được để trống");

    }
}
public class Response
{
    public string Message { get; set; }
}