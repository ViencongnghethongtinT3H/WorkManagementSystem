namespace WorkManagementSystem.Features.User.GetListUserForSelect;

public class Request
{
    public Guid? DepartmentId { get; set; }
}
public class UserModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }  // tên user
    //public Guid DepartmentId { get; set; }  // Phòng ban
    //public string DepartmentName { get; set; }  // Phòng ban
    public Guid PositionId { get; set; }  // Vị trí
    public string PositionName { get; set; }  // Vị trí
    //public string Email { get; set; }
    //public string? Phone { get; set; }
    public string NameAndPosition { get; set; }
    public string DeparmentName { get; set; }
}
