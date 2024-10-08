namespace WorkManagementSystem.Features.Folder.UpdateFolder
{
    public class Endpoint : Endpoint<Request, ResultModel<System.Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Patch("/folders/update/{Id}");
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            var data = new Data(_unitOfWork);
            var result = await data.UpdateFolder(r);
            await SendAsync(result);
        }


    }
}
