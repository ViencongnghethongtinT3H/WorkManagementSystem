namespace WorkManagementSystem.Features.WorkItem.CreateWorkItem;

public class Request : WorkItemModel
{
    public Guid UserCreateId { get; set; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Content)
             .NotEmpty().WithMessage("Nội dung trích yếu không được để trống!");
       // RuleFor(x => x.UserId).NotNull().WithMessage("Người phụ trách không được để trống");
    }
}
public class Response
{
    public string Message => "Work Item saved!";
    public string WorkItemId { get; set; }
}
