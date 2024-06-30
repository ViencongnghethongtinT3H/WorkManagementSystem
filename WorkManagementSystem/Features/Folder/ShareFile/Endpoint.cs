namespace WorkManagementSystem.Features.Folder.ShareFile
{
    public class Endpoint :  Endpoint<Request, ResultModel<FileManagement>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Post("/folders/share-file");
        }

       public override async Task HandleAsync(Request r, CancellationToken ct)
        {
              var data = new Data(_unitOfWork);
              var result = await data.ShareFile(r);
            await SendAsync(result); 
        }


    }
}
