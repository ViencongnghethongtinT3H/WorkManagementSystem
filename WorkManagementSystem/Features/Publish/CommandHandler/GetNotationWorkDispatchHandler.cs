namespace WorkManagementSystem.Features.Publish.CommandHandler
{
    public class GetNotationWorkDispatchHandler : ICommandHandler<GetNotationWorkDispatchCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetNotationWorkDispatchHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> ExecuteAsync(GetNotationWorkDispatchCommand command, CancellationToken ct)
        {
            var workDispatch = _unitOfWork.GetRepository<Entities.WorkDispatch>().GetAll();
            var setting = _unitOfWork.GetRepository<Entities.Setting>().GetAll();
            var notationName = (from w in workDispatch
                                join s in setting on w.Notation equals s.Key
                                where w.Id == command.WorkDispatchId
                                select new
                                {
                                    name = $"{w.ItemId}/{s.Value}"
                                }).FirstOrDefault(); 
            if (notationName != null)
            {
                return notationName.name;
            }
            return string.Empty;
        }

    }

}
