namespace WorkManagementSystem.Features.TaskDetail.CreateTaskDetail;

public class Request : TaskDetailModel
{
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Content)
             .NotEmpty().WithMessage("Nội dung trích yếu không được để trống!");          
    }
}
public class Response
{
public string Message => "Article saved!";
public string? ArticleID { get; set; }
}