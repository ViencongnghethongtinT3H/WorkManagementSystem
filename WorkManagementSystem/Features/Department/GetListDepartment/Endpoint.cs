﻿using WorkManagementSystem.Features.Setting;

namespace WorkManagementSystem.Features.Department.GetListDepartment;

public class Endpoint : Endpoint<Request, List<Reponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("setting/department/get-list-department/{level}");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);

        var setting = await data.GetDepartment(r.Level);

        if (setting is null)
            await SendNotFoundAsync();
        else
            await SendAsync(setting);
    }
}
