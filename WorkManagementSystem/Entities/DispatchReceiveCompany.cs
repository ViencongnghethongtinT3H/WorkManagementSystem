namespace WorkManagementSystem.Entities
{
    public class DispatchReceiveCompany : EntityBase
    {
        public Guid WorkDispatchId { get; set; }
        public Guid ReceiveCompanyId { get; set; }  // nơi nhận công văn
    }
}
