namespace WorkManagementSystem.Features.TaskDetail.GetTaskInCommingByCondition;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ListResultModel<TaskDetailResponse>> GetTaskByCondition(InputRequest input)
    {
        var taskRepo = _unitOfWork.GetRepository<Entities.TaskDetail>().GetAll();
        var workRepo = _unitOfWork.GetRepository<Entities.WorkItem>().GetAll();
        var settingRepo = _unitOfWork.GetRepository<Entities.Setting>().GetAll();
        var ImplemenRepo = _unitOfWork.GetRepository<Implementer>().GetAll();

        var query = from t in taskRepo.AsNoTracking()
                    join w in workRepo.AsNoTracking() on t.WorkItemId equals w.Id
                    join s3 in settingRepo.AsNoTracking() on w.Notation equals s3.Key into sd3
                    from b1 in sd3.DefaultIfEmpty()
                    join u in ImplemenRepo.AsNoTracking() on t.Id equals u.IssuesId
                    select new TaskDetailResponse
                    {
                        Id = t.Id,
                        WorkItemId = t.Id,
                        Content = t.Content,
                        Notation = $"{w.ItemId}/{b1.Value}",
                        ProcessingStatusValue = t.ProcessingStatus.GetDescription(),
                        ProcessingStatus = t.ProcessingStatus,
                        KeyWord = t.KeyWord,
                        DepartmentSentId = t.DepartmentSentId,
                        DepartmentReceiveId = t.DepartmentReceiveId,
                        Dealine = t.Dealine.ToFormatString("dd/MM/yyyy"),
                        UserReceiveId = u.UserReceiveId,
                        Created = t.Created
                    };

        var userId = input.Filters.GetFilterModel("UserId");
        if (userId is not null)
        {
            query = query.Where(x => x.UserReceiveId == new Guid(userId.FieldValue));
        }
        var workId = input.Filters.GetFilterModel("Id");
        if (workId is not null)
        {
            query = query.Where(x => x.Id == new Guid(workId.FieldValue));
        }

        var processingStatus = input.Filters.GetFilterModel("ProcessingStatus");
        if (processingStatus is not null)
        {
            query = query.Where(x => (int)x.ProcessingStatus == Convert.ToInt16(processingStatus.FieldValue));
        }

        var fromDate = input.Filters.GetFilterModel("FromDate");
        var toDate = input.Filters.GetFilterModel("ToDate");
        if (fromDate is not null && toDate is not null)
        {
            var fromValue = fromDate.FieldValue.ParseDateTimeNotNull(false, "dd/MM/yyyy");
            var toValue = toDate.FieldValue.ParseDateTimeNotNull(false, "dd/MM/yyyy");
            query = query.Where(x => x.Created > fromValue && x.Created <= toValue);
        }

        var data = new Response
        {
            Count = await query.CountAsync(),
            Items = await FilterActivitiesRepresentativeDetail(input, query)
        };
        return ListResultModel<TaskDetailResponse>.Create(data.Items, data.Count, input.Page, input.PageSize);

    }

    private async Task<List<TaskDetailResponse>> FilterActivitiesRepresentativeDetail(InputRequest r,
        IQueryable<TaskDetailResponse> query)
    {
        query = EntityQueryFilterHelper.CreateSort<TaskDetailResponse>(r.Sorts)(query);
        query = EntityQueryFilterHelper.Page<TaskDetailResponse>(r.Page, r.PageSize)(query);
        return await query.Distinct().ToListAsync();
    }

}
