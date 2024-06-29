﻿using WorkManagementSystem.Features.ToImplementer;

namespace WorkManagementSystem.Features.WorkDispatch.PublishWorkDispatch;

public class Endpoint : Endpoint<Request, ResultModel<Response>, Mapper>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventImplement _eventImplement;
    public Endpoint(IUnitOfWork unitOfWork, IEventImplement eventImplement)
    {
        _unitOfWork = unitOfWork;
        _eventImplement = eventImplement;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/WorkDispatch/publish-work-dispatch");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork, _eventImplement);
        var result = ResultModel<Response>.Create(new Response
        {
            WorkItemId = await data.CreateWorkDispatch(Map.ToEntity(r), r)
        });
        var name = await data.GetUserName(r);

        // Thêm phần lịch sử
        await new HistoryCommand
        {
            UserId = r.UserCompile,
            IssueId = new Guid(result.Data.WorkItemId),
            ActionContent = $"Tài khoản {name} đã tạo thêm một công văn"
        }.ExecuteAsync();

        if (string.IsNullOrEmpty(result.Data.WorkItemId))
            ThrowError("Không thể thêm công văn");
        await SendAsync(result);

    }
}
