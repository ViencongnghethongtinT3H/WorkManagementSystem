namespace WorkManagementSystem.Features.ToImplementer
{
    public interface IEventImplement
    {
         List<Implementer> ToImplementer(RequestImplementer r, Guid IssuesId);
    }
}
