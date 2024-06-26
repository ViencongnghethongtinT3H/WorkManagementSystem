namespace WorkManagementSystem.Features.WorkStep
{
    public class Mapper : Mapper<Request, Response, WorkflowStep>
    {
        public override WorkflowStep ToEntity(Request r) => new()
        {
            Note = r.Note,
            Step = r.Step,
            UserConfirm = r.UserConfirm,
            WorkflowId = r.WorkflowId,
        };
    }
}
