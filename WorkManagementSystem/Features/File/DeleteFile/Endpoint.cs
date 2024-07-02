namespace WorkManagementSystem.Features.File.DeleteFile
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
            Delete("/files/delete/{Id}");
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            var data = new Data(_unitOfWork);
            var result = await data.DeleteFile(r);
            await SendAsync(result);
        }


    }
}