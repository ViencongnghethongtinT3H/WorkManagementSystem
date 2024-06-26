﻿using WorkManagementSystem.Shared.Extensions;

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
                        DepartmentId = w.DepartmentId,
                        Created = w.Created,
                        UserIdCreated = w.UserIdCreated,
                        Dealine = w.Dealine
                    };
        var userId = input.Filters.GetFilterModel("UserId");
        // Lấy công văn của người tạo ra văn bản
        if (userId is not null)
        {
            query = query.Where(x => x.UserIdCreated == userId.FieldValue);

        }
        
        var workId = input.Filters.GetFilterModel("LeadershipDirectId");  // lãnh đạo chỉ đạo
        if (workId is not null)
        {
            query = query.Where(x => x.LeadershipId == new Guid(workId.FieldValue));
        }

        var processingStatus = input.Filters.GetFilterModel("ProcessingStatus");  // trạng thái
        if (processingStatus is not null)
        {
            query = query.Where(x => (int)x.ProcessingStatus == Convert.ToInt16(processingStatus.FieldValue));
        }
        var subjective = input.Filters.GetFilterModel("Subjective");  // chuyên đề
        if (subjective is not null)
        {
            query = query.Where(x => x.Subjective == subjective.FieldValue);
        }
        var keyWord = input.Filters.GetFilterModel("KeyWord");  // chuyên đề
        if (keyWord is not null)
        {
            query = query.Where(x => x.KeyWord == keyWord.FieldValue);
        }
        var departmentId = input.Filters.GetFilterModel("DepartmentId");  // phòng ban ban hành
        if (departmentId is not null)
        {
            query = query.Where(x => x.DepartmentId == new Guid(departmentId.FieldValue));
        }
        var fromDate = input.Filters.GetFilterModel("FromDate");
        var toDate = input.Filters.GetFilterModel("ToDate");
        if (fromDate is not null && toDate is not null)
        {
            var fromValue = fromDate.FieldValue.ParseDateTimeNotNull(false,"dd/MM/yyyy");
            var toValue = toDate.FieldValue.ParseDateTimeNotNull(false, "dd/MM/yyyy");
            query = query.Where(x => x.Created > fromValue && x.Created <= toValue);
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
