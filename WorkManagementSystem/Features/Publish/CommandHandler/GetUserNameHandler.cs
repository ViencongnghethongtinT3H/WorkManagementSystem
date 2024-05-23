
namespace WorkManagementSystem.Features.Publish.CommandHandler;

public class GetUserNameHandler : ICommandHandler<GetUserNameCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetUserNameHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> ExecuteAsync(GetUserNameCommand command, CancellationToken ct)
    {
        var user = await _unitOfWork.GetRepository<Entities.User>().GetAsync(command.UserId);
        if (user is not null)
        {
            return user.Name;
        }
        return string.Empty;
    }
}
