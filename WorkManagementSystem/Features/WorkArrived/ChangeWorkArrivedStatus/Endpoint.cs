namespace WorkManagementSystem.Features.WorkArrived.ChangeWorkArrivedStatus
{
    public class Endpoint : Endpoint<Request, ResultModel<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            AllowAnonymous();
            Post("/workArrived/change-work-arrived-status");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            var result = await data.ChangeWorkArrivedStatus(r);
            await SendAsync(result);
        }
    }
}
