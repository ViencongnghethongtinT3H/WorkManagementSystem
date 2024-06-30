namespace WorkManagementSystem.Features.WorkStep
{
    public class Endpoint : Endpoint<Request, ResultModel<Response>, Mapper>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            AllowAnonymous();
            Post("/WorkflowStep/create-or-update");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            var result = ResultModel<Response>.Create(new Response
            {
                result = await data.CreateWorkStep(Map.ToEntity(r), r)
            });
            await SendAsync(result);

        }
    }
}
