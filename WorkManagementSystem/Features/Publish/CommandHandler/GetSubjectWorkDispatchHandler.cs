namespace WorkManagementSystem.Features.Publish.CommandHandler
{
    public class GetSubjectWorkDispatchHandler : ICommandHandler<GetSubjectWorkDispatchCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetSubjectWorkDispatchHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> ExecuteAsync(GetSubjectWorkDispatchCommand command, CancellationToken ct)
        {
            var workDispatch = await _unitOfWork.GetRepository<Entities.WorkDispatch>().GetAsync(command.WorkDispatchId);
            if (workDispatch is not null)
            {
                return workDispatch.Subjective;
            }
            return string.Empty;
        }
    }

}
