namespace WorkManagementSystem.Features.Meeting.GetByIdMeeting
{
    public class Endpoint : Endpoint<Request, ResultModel<Response>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            AllowAnonymous();
            Get("/Meeting/get-by-id");
        }

        public override async Task HandleAsync(Request query, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            await SendAsync(await data.GetById(query));
        }
    }
}
