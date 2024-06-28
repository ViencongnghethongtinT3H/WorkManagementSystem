namespace WorkManagementSystem.Features.File.ShareFile
{
    public class Endpoint :  Endpoint<Request, ResultModel<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Post("/files/share-file");
        }

       public override async Task HandleAsync(Request r, CancellationToken ct)
        {
              var data = new Data(_unitOfWork);
              var result = await data.ShareFile(r);
            await SendAsync(result); 
        }


    }
}
