using FastEndpoints;
using System.Security.Claims;
using WorkManagementSystem.Infrastructure.UnitOfWork;

namespace WorkManagementSystem.Features.TaskDetail.CreateTaskDetail
{
    public class Endpoint : Endpoint<Request, Response, Mapper>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Endpoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public override void Configure()
        {
            Post("api/task/save-task");
            AllowAnonymous();
           
            // Claims(Claim.AuthorID);
            AccessControl(
                keyName: "Article_Save_Own",
                behavior: Apply.ToThisEndpoint,
                groupNames: "task");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var movieRepository = _unitOfWork.GetRepository<Entities.TaskDetail>();
            //var taskDetail = await movieRepository.
            Response.ArticleID = "";

            if (Response.ArticleID is null)
                ThrowError("Unable to save the article!");

            await SendAsync(Response);
        }
    }
}
