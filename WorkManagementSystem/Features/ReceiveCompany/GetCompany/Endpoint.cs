namespace WorkManagementSystem.Features.ReceiveCompany.GetCompany
{
    public class Endpoint : EndpointWithoutRequest<ResultModel<List<Response>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            Get("/receiveCompany/get-list-data");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var data = new Data(_unitOfWork);
            var result = await data.GetList();
            await SendAsync(ResultModel<List<Response>>.Create(result));

        }

    }
}
