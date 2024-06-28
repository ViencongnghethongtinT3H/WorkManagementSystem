namespace WorkManagementSystem.Features.Folder.GetListFolder
{
    public class Endpoint : Endpoint<Request, ListResultSelectModel<Response>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Get("/folders/get-folders");
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            var data = new Data(_unitOfWork);
            var folders = await data.GetFolder(r);
            var result = ListResultSelectModel<Response>.Create(folders);

            if (result is null)
                await SendNotFoundAsync();
            else
                await SendAsync(result);
        }


    }
}
