﻿using FluentValidation;
using WorkManagementSystem.Features.TaskDetail;

namespace WorkManagementSystem.Features.WorkItem;

public class Request : WorkItemModel
{
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Content)
             .NotEmpty().WithMessage("Nội dung trích yếu không được để trống!");
        RuleFor(x => x.UserId).NotNull().WithMessage("Người phụ trách không được để trống");
    }
}
public class Response
{
    public string Message => "Article saved!";
    public string? ArticleID { get; set; }
}
