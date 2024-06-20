namespace WorkManagementSystem.Features.ReceiveCompany.GetCompany;

public class Request
{
    public string name { get; set; }
}

public class Response
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? Updated { get; set; }
    public string? UserIdCreated { get; set; }
    public string? UserIdUpdated { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Active;
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Fax { get; set; }
    public string? Address { get; set; }
}
