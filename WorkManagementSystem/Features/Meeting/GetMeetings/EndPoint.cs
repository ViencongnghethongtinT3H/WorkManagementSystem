namespace WorkManagementSystem.Features.Meeting.GetMeetings
{
    public class Endpoint : Endpoint<Request, ListResultModel<Response>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            AllowAnonymous();
            Get("/Meeting/get-meet-by-condition");
        }

        public override async Task HandleAsync(Request query, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            var data1 = await data.GetListMeetings(query);
            await SendAsync(data1);
        }
    }
}
