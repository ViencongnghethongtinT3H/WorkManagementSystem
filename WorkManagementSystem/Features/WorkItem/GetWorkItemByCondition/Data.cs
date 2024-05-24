namespace WorkManagementSystem.Features.WorkItem.GetWorkItemByCondition;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ListResultModel<WorkItemResponse>> GetWorkItemByCondition(InputRequest input)
    {
        var work = _unitOfWork.GetRepository<Entities.WorkItem>().GetAll();
        var setting = _unitOfWork.GetRepository<Entities.Setting>().GetAll();
        var user = _unitOfWork.GetRepository<Entities.User>().GetAll();

        var query = from w in work.AsNoTracking()
                    join s1 in setting.AsNoTracking() on w.DocumentTypeKey equals s1.Key into cd
                    from b in cd.DefaultIfEmpty()
                    join s3 in setting.AsNoTracking() on w.Notation equals s3.Key into sd3
                    from b1 in sd3.DefaultIfEmpty()
                    join u in user.AsNoTracking() on w.LeadershipDirectId equals u.Id into ud
                    from b2 in ud.DefaultIfEmpty()
                    select new WorkItemResponse
                    {
                        WorkItemId = w.Id,
                        Content = w.Content,
                        Notation = $"{w.ItemId}/{b1.Value}",
                        ProcessingStatusValue = w.ProcessingStatus.GetDescription(),
                        ProcessingStatus = w.ProcessingStatus,
                        DateIssued = w.DateIssued.ToFormatString("dd/MM/yyyy"),
                        Priority = w.Priority,
                        PriorityValue = w.Priority.GetDescription(),
                        DocumentTypeValue = b.Value,
                        LeadershipDirect = b2.Name,
                        UserId = w.UserId,
                        LeadershipId = w.LeadershipDirectId,
                        Subjective = w.Subjective,
                        DepartmentId = w.DepartmentId
                    };
        var userId = input.Filters.GetFilterModel("UserId");
        if (userId is not null)
        {
            var id = new Guid(userId.FieldValue);
            var uid = new List<Guid> { new Guid("ba8655f7-c7e8-41b3-8520-08dc78ea81a9"), new Guid("95ab46e4-0f8c-4f17-8523-08dc78ea81a9") };
            // Nếu user là thuky hoặc lãnh đạo thì xem đc toàn bộ CV,
            // còn nếu ko thì chỉ xem đc công văn assign cho mình, hoặc người chủ trì CV, lãnh đạo chỉ đạo sẽ nhìn thấy CV 
            if (!uid.Contains(id))
            {
                query = query.Where(x => x.UserId == id || x.LeadershipId == id);
            }
        }
        var workId = input.Filters.GetFilterModel("Id");
        if (workId is not null)
        {
            query = query.Where(x => x.WorkItemId == new Guid(workId.FieldValue));
        }

        var processingStatus = input.Filters.GetFilterModel("ProcessingStatus");
        if (processingStatus is not null)
        {
            query = query.Where(x => (int)x.ProcessingStatus == Convert.ToInt16(processingStatus.FieldValue));
        }
        var subjective = input.Filters.GetFilterModel("Subjective");
        if (subjective is not null)
        {
            query = query.Where(x => x.Subjective == subjective.FieldValue);
        }
        var departmentId = input.Filters.GetFilterModel("DepartmentId");
        if (subjective is not null)
        {
            query = query.Where(x => x.DepartmentId == new Guid(departmentId.FieldValue));
        }


        var data = new Response
        {
            Count = await query.CountAsync(),
            Items = await FilterActivitiesRepresentativeDetail(input, query)
        };
        return ListResultModel<WorkItemResponse>.Create(data.Items, data.Count, input.Page, input.PageSize);

    }

    private async Task<List<WorkItemResponse>> FilterActivitiesRepresentativeDetail(InputRequest r,
        IQueryable<WorkItemResponse> query)
    {
        query = EntityQueryFilterHelper.CreateSort<WorkItemResponse>(r.Sorts)(query);
        query = EntityQueryFilterHelper.Page<WorkItemResponse>(r.Page, r.PageSize)(query);
        return await query.ToListAsync();
    }

}
