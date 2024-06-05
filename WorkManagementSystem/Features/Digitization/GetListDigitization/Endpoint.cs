namespace WorkManagementSystem.Features.Digitization.GetListDigitization
{
    public class Endpoint : Endpoint<Request, ResultModel<List<Response>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            Post("/digitization/get-list-data");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var data = new Data(_unitOfWork);
            var result = await data.GetList(req);
            await SendAsync(ResultModel<List<Response>>.Create(result));

        }

    }
}
