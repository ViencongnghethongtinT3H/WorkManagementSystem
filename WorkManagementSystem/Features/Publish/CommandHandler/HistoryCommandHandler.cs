namespace WorkManagementSystem.Features.Publish.CommandHandler;

public class HistoryCommandHandler : ICommandHandler<HistoryCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public HistoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(HistoryCommand command, CancellationToken ct)
    {
        var history = new Entities.History
        {
            actionContent = command.ActionContent,
            UserId = command.UserId,
            IssueId = command.IssueId,
        };
           
        var repo = _unitOfWork.GetRepository<Entities.History>();
        await repo.AddAsync(history);
        await _unitOfWork.CommitAsync();
        return true;
    }
}
