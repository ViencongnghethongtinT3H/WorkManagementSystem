namespace WorkManagementSystem.Features.WorkItem.GetWorkItemByCondition;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Response> GetWorkItemByCondition(InputRequest input)
    {
        var workItem = _unitOfWork.GetRepository<Entities.WorkItem>().GetAll().Select (x => new WorkItemModel
        {
            Content = x.Content,
            KeyWord = x.KeyWord,
            Notation = x.Notation,
            Priority = x.Priority,
            Subjective = x.Subjective,
            DateIssued = x.DateIssued,
            DepartmentId = x.DepartmentId,
            ProcessingStatus = x.ProcessingStatus,
            Status = x.Status,
            CategoryId = x.CategoryId,
            DocumentTypeKey = x.DocumentTypeKey,    
            ItemId = x.ItemId,  

        });

        var processingStatus = input.Filters.GetFilterModel("ProcessingStatus");
        if (processingStatus is not null)
        {
            workItem = workItem.Where(x => (int)x.ProcessingStatus == Convert.ToInt16(processingStatus.FieldValue));
        }
        var status = input.Filters.GetFilterModel("Status");
        if (status is not null)
        {
            workItem = workItem.Where(x => (int)x.Status == Convert.ToInt16(status.FieldValue));
        }
        var data = await FilterActivitiesRepresentativeDetail(input, workItem);
        
        return new Response
        {
            Count = await workItem.CountAsync(),
            Items = await FilterActivitiesRepresentativeDetail(input, workItem)
        };

    }

    private async Task<List<WorkItemModel>> FilterActivitiesRepresentativeDetail(InputRequest r,
        IQueryable<WorkItemModel> query)
    {
        query = EntityQueryFilterHelper.CreateSort<WorkItemModel>(r.Sorts)(query);
        query = EntityQueryFilterHelper.Page<WorkItemModel>(r.Page, r.PageSize)(query);
        return await query.ToListAsync();
    }
}
