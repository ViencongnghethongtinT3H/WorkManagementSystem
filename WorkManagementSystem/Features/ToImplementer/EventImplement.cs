namespace WorkManagementSystem.Features.ToImplementer
{
    public class EventImplement : IEventImplement
    {

        public List<Implementer> ToImplementer(RequestImplementer r, Guid IssuesId)
        {
            var lst = new List<Implementer>();
            foreach (var item in r.Implementers)
            {
                lst.Add(new Implementer
                {
                    IssuesId = IssuesId,
                    ProgressValue = 0,
                    IsTaskItem = false,
                    UserReceiveId = item.UserReceiveId,
                    Note = item.Note,
                    DepartmentReceiveId = item.DepartmentReceiveId
                });
            }
            return lst;
        }
    }
}
