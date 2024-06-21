namespace WorkManagementSystem.Features.WorkDispatch.ApproveWorkDispatch
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
            Post("/workDispatch/ApproveWorkDispatch");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            var result = await data.ApproveWorkDispatch(r);
            await SendAsync(result);
        }
    }
}
