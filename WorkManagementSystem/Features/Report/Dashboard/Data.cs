namespace WorkManagementSystem.Features.Report.Dashboard;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<DashBoardViewMode>> GetHistoryByIssuesId(Request r)
    {
        //var history = _unitOfWork.GetRepository<Entities.History>().GetAll();
        //var users = _unitOfWork.GetRepository<Entities.User>().GetAll();
        //return await (from h in history
        //              join u in users on h.UserId equals u.Id
        //              where h.IssueId == r.IssuesId
        //              orderby h.Created descending
        //              select new Response
        //              {
        //                  ActionContent = h.actionContent,
        //                  ActionTime = h.ActionTime,
        //                  UserUpdated = u.Name
        //              }).ToListAsync();
        return null;

    }
}
