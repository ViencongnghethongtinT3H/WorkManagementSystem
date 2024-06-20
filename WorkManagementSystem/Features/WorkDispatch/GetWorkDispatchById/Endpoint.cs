namespace WorkManagementSystem.Features.WorkDispatch.GetWorkDispatchById
{
    public class Endpoint : Endpoint<Request, ResultModel<WorkDispatchDetailResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            AllowAnonymous();
            Get("WorkDispatch/get-work-dispatch-by-id");
        }

        public override async Task HandleAsync(Request query, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            await SendAsync(await data.GetWorkItemById(query));
        }
    }
}
