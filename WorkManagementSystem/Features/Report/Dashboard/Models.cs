namespace WorkManagementSystem.Features.Report.Dashboard;

public class Request
{
    public Guid UserId { get; set; }
}
public class DashBoardViewMode
{
    public List<TaskItemReport> TaskItemReports { get; set; }
    public List<WorkItemReport> WorkItemReports { get; set; }

}

public class TaskItemReport
{
    public int CountTasks { get; set; }
    public int TaskStatusIsDone { get; set; }
    public int TaskStatusIsProgress { get; set; }
    public int TaskStatusIsToDo { get; set; }
}

public class WorkItemReport
{
    public int CountWorkItems { get; set; }
    public int WorkItemIsDone { get; set; }
    public int WorkItemIsProgress { get; set; }
    public int WorkItemIsToDo { get; set; }
}

