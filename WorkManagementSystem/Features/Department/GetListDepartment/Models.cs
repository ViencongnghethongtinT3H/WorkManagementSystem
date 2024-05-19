namespace WorkManagementSystem.Features.Department.GetListDepartment;

public class Request
{
    public OrganizationLevelEnum Level { get; set; }
}
public class Reponse
{
    public DepartmentModel Department { get; set; }
    public List<Reponse> Children { get; set; } = new List<Reponse>();
}
