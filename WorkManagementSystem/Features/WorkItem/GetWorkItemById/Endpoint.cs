using WorkManagementSystem.Features.WorkItem.GetWorkItemByCondition;

namespace WorkManagementSystem.Features.WorkItem.GetWorkItemById
{
    public class Endpoint : Endpoint<Request, ResultModel<WorkItemDetailResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            AllowAnonymous();
            Get("WorkItem/get-work-item-by-id");
        }

        public override async Task HandleAsync(Request query, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            await SendAsync(await data.GetWorkItemById(query));
        }
    }

}
