namespace WorkManagementSystem.Features.WorkArrived.GetListWorkDispatchWattingArrive
{
    public class Endpoint : Endpoint<Request, ListResultModel<WorkArriveResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            AllowAnonymous();
            Get("WorkDispatch/get-list-work-watting-arrive");
        }

        public override async Task HandleAsync(Request query, CancellationToken c)
        {
            var queryModel = HttpContext.SafeGetListQuery<InputRequest, Response>(query.Query);
            var data = new Data(_unitOfWork);
            await SendAsync(await data.GetListWorkDispatchWattingWork(queryModel));
        }
    }
}
