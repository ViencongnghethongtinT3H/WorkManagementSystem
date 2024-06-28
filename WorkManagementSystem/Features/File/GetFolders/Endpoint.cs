namespace WorkManagementSystem.Features.File.GetFolders
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
            AllowAnonymous();
            Get("/folders/get-folders");
        }

       public override async Task HandleAsync(Request r, CancellationToken ct)
        {
              var data = new Data(_unitOfWork);
              var result = await data.GetFolder(r);
              await SendAsync(ResultModel<List<Response>>.Create(result));       
        }


    }
}